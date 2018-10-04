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
            return TBuiltInTypes.BOOLEAN.Instantiate(left.ToBoolean() || right.ToBoolean());
        }

        internal override string Operator()
        {
            return "||";
        }
    }
}