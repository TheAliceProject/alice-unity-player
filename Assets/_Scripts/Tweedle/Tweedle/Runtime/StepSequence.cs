using System.Collections.Generic;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class StepSequence : ExecutionStep
    {
        List<ExecutionStep> steps = new List<ExecutionStep>();
        int index = 0;

        public StepSequence(string callStackEntry, ExecutionScope scope)
            : base(scope)
        {
            this.callStack = scope.StackWith(callStackEntry);
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