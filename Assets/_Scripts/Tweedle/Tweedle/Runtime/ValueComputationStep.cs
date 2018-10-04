using System;
using Alice.Tweedle;

namespace Alice.Tweedle.VM
{
    class ValueComputationStep : ExecutionStep
    {
        TValue initialValue;
        Func<TValue, TValue> body;

        public ValueComputationStep(string callStackEntry, ExecutionScope scope, Func<TValue, TValue> body)
            : base(scope)
        {
            this.body = body;
            this.callStack = scope.StackWith(callStackEntry);
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