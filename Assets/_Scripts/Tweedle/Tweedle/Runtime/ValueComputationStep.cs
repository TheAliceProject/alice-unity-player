using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    class ValueComputationStep : ExecutionStep
    {
        TValue initialValue;
        Func<TValue, TValue> body;

        public ValueComputationStep(string callStackEntry, ExecutionScope scope, Func<TValue, TValue> body)
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
            result = body.Invoke(initialValue);
            base.Execute();
        }
    }
}