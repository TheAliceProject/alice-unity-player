namespace Alice.Tweedle
{
    class SubtractionExpression : BinaryExpression
    {

        public SubtractionExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue lValue, TweedleValue rValue)
        {
            throw new System.NotImplementedException();
        }
    }
}