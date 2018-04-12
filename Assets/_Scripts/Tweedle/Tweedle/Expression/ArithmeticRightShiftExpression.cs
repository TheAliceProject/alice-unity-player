namespace Alice.Tweedle
{
    class ArithmeticRightShiftExpression : BinaryExpression
    {

        public ArithmeticRightShiftExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue lValue, TweedleValue rValue)
        {
            throw new System.NotImplementedException();
        }
    }
}