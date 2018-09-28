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
        [PInteropMethod]
        static public AsyncReturn delay(double duration)
        {
            AsyncReturn returnVal = new AsyncReturn();
            if (duration <= 0) {
                returnVal.Return();
                return returnVal;
            }

            
            UnitySceneGraph.Current.StartCoroutine(DelayImpl(returnVal, duration));
            return returnVal;
        }

        [PInteropMethod]
        static public AsyncReturn<bool> returnRandomBool(double duration)
        {
            AsyncReturn<bool> returnVal = new AsyncReturn<bool>();
            UnitySceneGraph.Current.StartCoroutine(ReturnWithDelay(returnVal, duration, Random.value < 0.5));
            return returnVal;
        }

        static private IEnumerator DelayImpl(AsyncReturn inReturn, double inDuration)
        {
            yield return new WaitForSeconds((float)inDuration);
            inReturn.Return();
        }

        static private IEnumerator ReturnWithDelay<T>(AsyncReturn<T> inReturn, double inDuration, T inValue)
        {
            yield return new WaitForSeconds((float)inDuration);
            inReturn.Return(inValue);
        }
    }
}