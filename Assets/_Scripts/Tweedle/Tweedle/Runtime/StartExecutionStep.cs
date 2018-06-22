using System;
using Alice.VM;

namespace Alice.Tweedle
{
    internal class StartExecutionStep : ExecutionStep
    {
        bool started = false;
        Func<ExecutionStep> generator;
        ExecutionStep child;

        public StartExecutionStep(string callStack, Func<ExecutionStep> generator)
        {
            this.generator = generator;
            this.callStack = callStack;
        }

        public StartExecutionStep(ExecutionStep blockingStep, Func<ExecutionStep> generator)
            : base(blockingStep)
        {
            this.generator = generator;
        }

        internal override bool Execute()
        {
            if (!started)
            {
                started = true;
                child = generator.Invoke();
                AddBlockingStep(child);
                return false;
            }
            else
            {
                return MarkCompleted();
            }
        }
    }
}