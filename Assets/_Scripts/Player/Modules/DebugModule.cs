using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;

namespace Alice.Player.Modules
{
    [PInteropType("Debug")]
    static public class DebugModule
    {
        [PInteropMethod]
        static public void log(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        [PInteropMethod]
        static public void dump(TValue @object)
        {
            UnityEngine.Debug.Log(@object.ToString());
        }
    }
}