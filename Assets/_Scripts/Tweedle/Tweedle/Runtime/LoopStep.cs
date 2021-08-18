using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    // TODO Abstract more of the looping behavior from the subclasses: While, Count, and Array
    public class LoopStep<T> : ExecutionStep
        where T : AbstractLoop
    {
        protected T statement;

        public LoopStep(T statement, ExecutionScope scope, ExecutionStep next)
            : base(null, scope, next)
        {
            this.statement = statement;
        }
    }
}