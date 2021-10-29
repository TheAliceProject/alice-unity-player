using System.Collections.Generic;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    class LambdaScope : InvocationScope
    {
        private TLambda lambda;
        private ExecutionScope capturedScope;

        private static IStackFrame lamdaCompletedStackFrame = new StaticStackFrame("Lambda Completed");

        internal LambdaScope(ExecutionScope caller)
            : base(null, caller)
        {
        }

        internal void SetLambda(TLambda lambda)
        {
            this.lambda = lambda;
            this.capturedScope = lambda.CapturedScope;
            this.thisValue = capturedScope.GetThis();
        }

        protected override bool TryGetParentValue(string varName, out TValue outValue)
        {
            if (capturedScope != null)
            {
                outValue = capturedScope.GetValue(varName);
                return true;
            }

            outValue = TValue.UNDEFINED;
            return false;
        }

        protected override bool UpdateParentValue(string varName, TValue value)
        {
            if (capturedScope != null)
            {
                capturedScope.SetValue(varName, value);
                return true;
            }

            return false;
        }

        internal override ExecutionStep InvocationStep(IStackFrame callStackEntry, NamedArgument[] arguments)
        {
            return base.InvocationStep(callStackEntry, arguments);
        }

        internal void QueueInvocationStep(StepSequence sequentialSteps, ITweedleExpression[] arguments, AsyncReturn<TValue> returnVal)
        {
            lambda.AddInvocationSteps(this, sequentialSteps, arguments);
            if (returnVal != null)
            {
                sequentialSteps.AddStep(new DelayedOperationStep(
                    lamdaCompletedStackFrame,
                    this,
                    () =>
                    {
                        returnVal.Return(Result);
                    }
                ));
            }
        }
    }
}
