namespace Alice.Tweedle
{
    class ModuloExpression : BinaryExpression
    {

        public ModuloExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue lValue, TweedleValue rValue)
        {
            throw new System.NotImplementedException();
        }
    }
}