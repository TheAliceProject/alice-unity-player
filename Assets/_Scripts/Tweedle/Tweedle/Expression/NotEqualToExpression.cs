namespace Alice.Tweedle
{
    class NotEqualToExpression : BinaryExpression
    {

        public NotEqualToExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.BOOLEAN)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
        {
			return TweedleTypes.BOOLEAN.Instantiate(!left.Equals(right));
        }
    }
}