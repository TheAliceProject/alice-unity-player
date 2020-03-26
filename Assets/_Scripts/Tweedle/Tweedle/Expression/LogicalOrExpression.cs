using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    class LogicalOrExpression : BinaryExpression
    {
        public LogicalOrExpression(ITweedleExpression lhs, ITweedleExpression rhs)
            : base(lhs, rhs, TBuiltInTypes.BOOLEAN)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        protected override TValue Evaluate(TValue left, TValue right)
        {
            return TBuiltInTypes.BOOLEAN.Instantiate(left.ToBoolean() || right.ToBoolean());
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            return new ShortCircuitingTwoValueComputationStep(ToTweedle(), scope, lhs, rhs, Evaluate, TValue.TRUE);
        }

        internal override string Operator()
        {
            return "||";
        }
    }
}