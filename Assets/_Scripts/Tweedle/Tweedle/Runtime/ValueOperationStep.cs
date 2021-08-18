using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    public class ValueOperationStep : ExecutionStep
    {
        TValue initialValue;
        Action<TValue> body;

        public ValueOperationStep(IStackFrame callStackEntry, ExecutionScope scope, Action<TValue> body)
            : base(callStackEntry, scope)
        {
            this.body = body;
        }

        internal override void BlockerFinished(ExecutionStep blockingStep)
        {
            base.BlockerFinished(blockingStep);
            initialValue = blockingStep.Result;
        }

        internal override void Execute()
        {
            body.Invoke(initialValue);
            base.Execute();
        }
    }
}