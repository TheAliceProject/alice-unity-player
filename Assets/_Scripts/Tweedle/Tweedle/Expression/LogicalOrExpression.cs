namespace Alice.Tweedle
{
    class LogicalOrExpression : BinaryExpression
    {

        public LogicalOrExpression(ITweedleExpression lhs, ITweedleExpression rhs)
            : base(lhs, rhs, TBuiltInTypes.BOOLEAN)
        {
        }

        protected override TValue Evaluate(TValue left, TValue right)
        {
            if (left.ToBoolean())
                return TBuiltInTypes.BOOLEAN.Instantiate(true);
            else
                return TBuiltInTypes.BOOLEAN.Instantiate(right.ToBoolean());
        }

        internal override string Operator()
        {
            return "||";
        }
    }
}