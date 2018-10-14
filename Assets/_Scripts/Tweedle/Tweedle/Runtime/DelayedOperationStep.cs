using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    // A step to execute an action only after all preceeding steps are complete.
    // Used to invoke methods, constructors, and lambdas.
    class DelayedOperationStep : ExecutionStep
    {
        Action body;

        public DelayedOperationStep(string callStackEntry, ExecutionScope scope, Action body)
            : base(scope)
        {
            using (PooledStringBuilder stackBuilder = PooledStringBuilder.Alloc(callStackEntry)) {
                scope.StackWith(stackBuilder.Builder);
                this.callStack = stackBuilder.ToString();
            }
            this.body = body;
        }

        internal override void Execute()
        {
            body.Invoke();
            base.Execute();
        }
    }
}