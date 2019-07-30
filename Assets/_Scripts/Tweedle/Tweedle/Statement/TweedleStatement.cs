using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

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
                try
                {
                    AsStepToNotify(scope, next).Queue();
                }
                catch (TweedleRuntimeException tre)
                {
                    UnityEngine.Debug.LogErrorFormat("Executing {0} triggered error {1}", this, tre);
                }
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