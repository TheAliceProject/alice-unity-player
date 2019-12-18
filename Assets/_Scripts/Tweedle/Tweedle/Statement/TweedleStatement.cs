using Alice.Tweedle.VM;
using Alice.Utils;

namespace Alice.Tweedle
{
    abstract public class TweedleStatement
    {
        bool enabled = true;

        public bool IsEnabled
        { get { return enabled; } }

        internal void Disable()
        {
            enabled = false;
        }

        internal virtual void QueueStepToNotify(ExecutionScope scope, ExecutionStep next)
        {
            if (enabled)
            {
                AsStepToNotify(scope, next).Queue();
            }
            else if (next != null)
            {
                next.Queue();
            }
        }

        internal abstract ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next);

        static public readonly TweedleStatement[] EMPTY_STATEMENTS = new TweedleStatement[0];
    }
}