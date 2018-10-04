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

        [PInteropMethod]
        static public AsyncReturn delay(double duration)
        {
            AsyncReturn returnVal = new AsyncReturn();
            if (duration <= 0) {
                returnVal.Return();
                return returnVal;
            }

            UnitySceneGraph.Current.QueueTimeReturn(returnVal, duration);
            return returnVal;
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

            UnitySceneGraph.Current.StartCoroutine(CallOnDelay(lambda, delay));
        }

        [PInteropMethod]
        public static AsyncReturn delayOneFrame() {
            AsyncReturn returnValue = new AsyncReturn();
            UnitySceneGraph.Current.QueueFrameReturn(returnValue, 1);
            return returnValue;
        }

        [PInteropMethod]
        public static AsyncReturn delayFrames(double frames) {
            if (frames <= 0) {
                return null;
            }

            AsyncReturn returnValue = new AsyncReturn();
            UnitySceneGraph.Current.QueueFrameReturn(returnValue, (int)frames);
            return returnValue;
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