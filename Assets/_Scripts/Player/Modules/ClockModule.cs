using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    [PInteropType("Clock")]
    static public class ClockModule
    {
        [PInteropField]
        static public double currentTime { get { return UnityEngine.Time.time; } }

        [PInteropField]
        static public double deltaTime { get { return UnityEngine.Time.deltaTime; } }

        [PInteropField]
        static public double simulationSpeedFactor { 
            get { return UnityEngine.Time.timeScale; }
            set {
                // max time scale for Unity is 100
                float scale =   Mathf.Min(100f, (float)value);
                UnityEngine.Time.timeScale = scale;
            }
        }

        [PInteropMethod]
        static public AsyncReturn delay(double duration)
        {
            if (duration <= 0) {
                return null;
            }

            AsyncReturn returnVal = new AsyncReturn();
            SceneGraph.Current.QueueTimeReturn(returnVal, duration);
            return returnVal;
        }

        [PInteropMethod]
        public static AsyncReturn delayOneFrame() {
            AsyncReturn returnValue = new AsyncReturn();
            SceneGraph.Current.QueueFrameReturn(returnValue, 1);
            return returnValue;
        }

        [PInteropMethod]
        public static AsyncReturn delayFrames(double frames) {
            if (frames <= 0) {
                return null;
            }

            AsyncReturn returnValue = new AsyncReturn();
            SceneGraph.Current.QueueFrameReturn(returnValue, (int)frames);
            return returnValue;
        }

        [PInteropMethod]
        static public void callOnDelay(PFunc<double, double, double> lambda, double delay)
        {
            if (delay <= 0)
            {
                var res = lambda.Call(Time.time, Time.time);
                res.OnReturn((d) =>
                {
                    UnityEngine.Debug.Log("[ClockModule] Got result back of " + d);
                });
                return;
            }

            SceneGraph.Current.StartCoroutine(CallOnDelay(lambda, delay));
        }

        static private IEnumerator CallOnDelay(PFunc<double, double, double> lambda, double inDelay)
        {
            double initialTime = Time.time;
            yield return new WaitForSeconds((float)inDelay);
            var res = lambda.Call(initialTime, Time.time);
            res.OnReturn((d) =>
            {
                UnityEngine.Debug.Log("[ClockModule] Got result back of " + d);
            });
        }
    }
}