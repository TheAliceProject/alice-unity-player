using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    abstract public class MemberAccessExpression : TweedleExpression
    {
        public ITweedleExpression Target { get; }
        internal protected bool invokeSuper;

        public MemberAccessExpression(ITweedleExpression target)
            : base(null)
        {
            Target = target;
            invokeSuper = false;
        }

        internal ExecutionStep TargetStep(ExecutionScope scope)
        {
            return Target.AsStep(scope);
        }
    }
}