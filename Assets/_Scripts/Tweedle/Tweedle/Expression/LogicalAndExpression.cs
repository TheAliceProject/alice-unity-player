namespace Alice.Tweedle
{
    class LogicalAndExpression : BinaryExpression
    {

        public LogicalAndExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue lValue, TweedleValue rValue)
        {
            throw new System.NotImplementedException();
        }
    }
}