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
                try {
                    AsStepToNotify(scope, next).Queue();
                } catch (TweedleRuntimeException tre) {
                    using (PooledStringBuilder stackBuilder = PooledStringBuilder.Alloc())
                    {
                        scope.StackWith(stackBuilder.Builder);
                        UnityEngine.Debug.LogErrorFormat("Statement {0} triggered error {1}\nTweedle stack:{2}\n", this, tre, stackBuilder.ToString());
                    }
                }
            } else if (next != null) {
                next.Queue();
            }
        }

        internal abstract ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next);

        static public readonly TweedleStatement[] EMPTY_STATEMENTS = new TweedleStatement[0];
    }
}