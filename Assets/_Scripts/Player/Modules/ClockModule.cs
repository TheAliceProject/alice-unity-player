using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using Alice.Player.Primitives;
using System.Collections;

namespace Alice.Player.Modules
{
    [PInteropType("Clock")]
    static public class ClockModule
    {
        [PInteropField]
        static public Duration currentTime { get { return new Duration(UnityEngine.Time.time); } }

        [PInteropField]
        static public Duration deltaTime { get { return new Duration(UnityEngine.Time.deltaTime); } }

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
        static public AsyncReturn delay(Duration duration)
        {
            if (duration.seconds <= 0) {
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
        public static AsyncReturn delayFrames(int frames) {
            if (frames <= 0) {
                return null;
            }

            AsyncReturn returnValue = new AsyncReturn();
            SceneGraph.Current.QueueFrameReturn(returnValue, frames);
            return returnValue;
        }
    }
}