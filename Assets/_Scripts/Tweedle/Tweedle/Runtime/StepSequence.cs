using System.Collections.Generic;
using Alice.Tweedle.VM;
using Alice.Utils;

namespace Alice.Tweedle
{
    public class StepSequence : ExecutionStep
    {
        List<ExecutionStep> steps = new List<ExecutionStep>();
        int index = 0;

        public StepSequence(IStackFrame callStackEntry, ExecutionScope scope)
            : base(callStackEntry, scope)
        {
        }

        internal void AddStep(ExecutionStep step)
        {
            steps.Add(step);
        }

        internal override void Execute()
        {
            if (index < steps.Count)
            {
                steps[index++].QueueAndNotify(this);
            }
            else
            {
                result = steps.Count > 0 ? steps[index - 1].Result : TValue.NULL;
                base.Execute();
            }
        }
    }
}