using System;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    class ValueGenerationStep : ExecutionStep
    {
        Func<TValue> body;

        public ValueGenerationStep(IStackFrame callStackEntry, ExecutionScope scope, Func<TValue> body)
            : base(callStackEntry, scope)
        {
            this.body = body;
        }

        internal override void Execute()
        {
            result = body.Invoke();
            base.Execute();
        }
    }
}