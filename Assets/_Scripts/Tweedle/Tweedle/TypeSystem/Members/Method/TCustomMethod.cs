using System.Collections.Generic;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Custom method with hard-coded interaction between tweedle and C#.
    /// </summary>
    public class TCustomMethod : TMethod
    {
        public delegate TValue MethodDelegate(ExecutionScope inScope);

        private MethodDelegate m_Method;

        public TCustomMethod(string inName, MemberFlags inFlags, TTypeRef inResultType, TParameter[] inRequiredParams, TParameter[] inOptionalParams, MethodDelegate inMethod)
        {
            m_Method = inMethod;

            SetupMember(inName, inResultType, inFlags);
            SetupParameters(inRequiredParams, inOptionalParams);
        }

        protected override void AddBodyStep(InvocationScope inScope, StepSequence ioMain)
        {
            ioMain.AddStep(
                new DelayedOperationStep("call", inScope, () => {
                    inScope.Result = m_Method(inScope);
                })
            );
        }
    }
}