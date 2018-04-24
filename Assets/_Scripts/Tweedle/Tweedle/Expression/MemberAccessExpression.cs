using Alice.VM;

namespace Alice.Tweedle
{
    abstract public class MemberAccessExpression : TweedleExpression
    {
        private TweedleExpression target;

        public TweedleExpression Target
        {
            get
            {
                return target;
            }
        }

        public MemberAccessExpression(TweedleExpression target)
            : base(null)
        {
            this.target = target;
        }

        protected TweedleValue EvaluateTarget(TweedleFrame frame)
        {
            return target.Evaluate(frame);
        }
    }
}