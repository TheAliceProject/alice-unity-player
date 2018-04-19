namespace Alice.Tweedle
{
    class EqualToExpression : BinaryExpression
    {

        public EqualToExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.BOOLEAN)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
        {
			return TweedleTypes.BOOLEAN.Instantiate(left.Equals(right));
        }
    }
}