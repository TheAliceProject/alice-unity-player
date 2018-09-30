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
            if (duration <= 0)
                return null;

            AsyncReturn returnVal = new AsyncReturn();
            UnitySceneGraph.Instance.StartCoroutine(DelayImpl(returnVal, duration));
            return returnVal;
        }

        [PInteropMethod]
        static public void callOnDelay(TValue lambda, double delay)
        {
            if (delay <= 0)
            {
                TInterop.QueueLambda(lambda.Lambda());
                return;
            }

            UnitySceneGraph.Instance.StartCoroutine(CallOnDelay(lambda.Lambda(), delay));
        }

        [PInteropMethod]
        static public AsyncReturn<bool> returnRandomBool(double duration)
        {
            AsyncReturn<bool> returnVal = new AsyncReturn<bool>();
            UnitySceneGraph.Instance.StartCoroutine(ReturnWithDelay(returnVal, duration, Random.value < 0.5));
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

        static private IEnumerator CallOnDelay(TLambda lambda, double inDelay)
        {
            yield return new WaitForSeconds((float)inDelay);
            TInterop.QueueLambda(lambda);
        }
    }
}