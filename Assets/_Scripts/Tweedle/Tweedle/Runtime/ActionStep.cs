using System;

namespace Alice.VM
{
    internal class ActionStep : ExecutionStep
    {
        Action action;

        public ActionStep(Action action)
        {
            this.action = action;
        }

        public ActionStep(ExecutionStep blockingStep, Action action)
            : base(blockingStep)
        {
            this.action = action;
        }

        internal override bool Execute()
        {
            action.Invoke();
            return MarkCompleted();
        }
    }
}