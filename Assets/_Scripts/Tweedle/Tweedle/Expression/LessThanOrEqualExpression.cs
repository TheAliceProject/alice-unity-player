namespace Alice.Tweedle
{
    class LessThanOrEqualExpression : BinaryExpression
    {

        public LessThanOrEqualExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue lValue, TweedleValue rValue)
        {
            throw new System.NotImplementedException();
        }
    }
}