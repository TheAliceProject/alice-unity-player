namespace Alice.Tweedle
{
    class LogicalAndExpression : BinaryExpression
    {

        public LogicalAndExpression(ITweedleExpression lhs, ITweedleExpression rhs)
            : base(lhs, rhs, TBuiltInTypes.BOOLEAN)
        {
        }

        protected override TValue Evaluate(TValue left, TValue right)
        {
            return TBuiltInTypes.BOOLEAN.Instantiate(left.ToBoolean() && right.ToBoolean());
        }

        protected override TValue Evaluate(ITweedleExpression left, ITweedleExpression right)
        {
            TValue leftValue = (TValue)left;
            if (leftValue.ToBoolean())
                return (TValue)right;
            else
                return leftValue;
        }

        internal override string Operator()
        {
            return "&&";
        }
    }
}