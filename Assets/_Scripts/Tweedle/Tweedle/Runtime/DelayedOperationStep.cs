using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    // A step to execute an action only after all preceding steps are complete.
    // Used to invoke methods, constructors, and lambdas.
    class DelayedOperationStep : ExecutionStep
    {
        Action body;

        public DelayedOperationStep(IStackFrame callStackEntry, ExecutionScope scope, Action body)
            : base(callStackEntry, scope)
        {
            this.body = body;
        }

        internal override void Execute()
        {
            body.Invoke();
            base.Execute();
        }
    }
}