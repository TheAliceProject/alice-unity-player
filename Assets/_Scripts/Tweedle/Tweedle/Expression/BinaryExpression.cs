namespace Alice.Tweedle
{
    public abstract class BinaryExpression : TweedleExpression
    {
        private TweedleExpression lhs;
        private TweedleExpression rhs;

        public BinaryExpression(TweedleExpression lhs, TweedleExpression rhs, TweedleType type)
			: base(type)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public override TweedleValue Evaluate(VM.TweedleFrame frame)
        {
            return Evaluate(lhs.Evaluate(frame), rhs.Evaluate(frame));
        }

        protected abstract TweedleValue Evaluate(TweedleValue left, TweedleValue right);
    }
}