using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform method.
    /// </summary>
    public class PAsyncMethod : PMethodBase
    {
        public PAsyncMethod(TAssembly inAssembly, MethodInfo inMethod)
            : base(inAssembly, inMethod, ExtractAsyncResultType(inMethod.ReturnType), MemberFlags.Async)
        {
        }

        static private Type ExtractAsyncResultType(Type inType)
        {
            if (inType.IsGenericType)
                return inType.GenericTypeArguments[0];

            return typeof(void);
        }

        protected override void AddBodyStep(InvocationScope inScope, StepSequence ioMain)
        {
            ioMain.AddStep(
                new DelayedAsyncOperationStep("call", inScope, () => {
                    object thisVal = PrepForInvoke(inScope);
                    IAsyncReturn asyncReturn = (IAsyncReturn)Invoke(thisVal, GetCachedArgs());
                    if (asyncReturn != null)
                    {
                        asyncReturn.OnReturn((obj) =>
                        {
                            ReturnValue(inScope, obj);
                        });
                    }
                    else
                    {
                        ReturnValue(inScope, null);
                    }
                    return asyncReturn;
                })
            );
        }
    }
}