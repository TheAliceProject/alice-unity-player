namespace Alice.Tweedle
{
    public class StringConcatenationExpression : BinaryExpression
    {

        public StringConcatenationExpression(ITweedleExpression lhs, ITweedleExpression rhs)
            : base(lhs, rhs, TBuiltInTypes.TEXT_STRING)
        {
        }

        protected override TValue Evaluate(TValue left, TValue right)
        {
            return TBuiltInTypes.TEXT_STRING.Instantiate(left.ToTextString() + right.ToTextString());
        }

        internal override string Operator()
        {
            return "..";
        }
    }
}