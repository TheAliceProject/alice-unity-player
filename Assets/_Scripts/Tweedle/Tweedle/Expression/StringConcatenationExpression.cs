using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class StringConcatenationExpression : BinaryExpression
    {

        public StringConcatenationExpression(ITweedleExpression lhs, ITweedleExpression rhs)
            : base(lhs, rhs, TBuiltInTypes.TEXT_STRING)
        {
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            return new TwoStringsStep(this, scope, lhs, rhs, Evaluate);
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