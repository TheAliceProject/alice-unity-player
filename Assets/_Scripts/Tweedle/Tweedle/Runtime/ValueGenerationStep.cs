using System;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    class ValueGenerationStep : ExecutionStep
    {
        Func<TValue> body;

        public ValueGenerationStep(string callStackEntry, ExecutionScope scope, Func<TValue> body)
            : base(scope)
        {
            using (PooledStringBuilder stackBuilder = PooledStringBuilder.Alloc(callStackEntry)) {
                scope.StackWith(stackBuilder.Builder);
                callStack = stackBuilder.ToString();
            }
            this.body = body;
        }

        internal override void Execute()
        {
            result = body.Invoke();
            base.Execute();
        }
    }
}