namespace Alice.Tweedle
{
    class LogicalRightShiftExpression : BinaryExpression
    {

        public LogicalRightShiftExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue lValue, TweedleValue rValue)
        {
            throw new System.NotImplementedException();
        }
    }
}