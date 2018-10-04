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

            UnitySceneGraph.Instance.StartCoroutine(CallOnDelay(lambda, delay));
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