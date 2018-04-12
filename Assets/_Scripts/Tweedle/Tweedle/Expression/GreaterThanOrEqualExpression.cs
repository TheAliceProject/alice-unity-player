namespace Alice.Tweedle
{
    class GreaterThanOrEqualExpression : BinaryExpression
    {

        public GreaterThanOrEqualExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue lValue, TweedleValue rValue)
        {
            throw new System.NotImplementedException();
        }
    }
}