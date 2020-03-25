using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    class LogicalAndExpression : BinaryExpression
    {
        ITweedleExpression lhs;
        ITweedleExpression rhs;

        public LogicalAndExpression(ITweedleExpression lhs, ITweedleExpression rhs)
            : base(lhs, rhs, TBuiltInTypes.BOOLEAN)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        protected override TValue Evaluate(TValue left, TValue right)
        {
            return TBuiltInTypes.BOOLEAN.Instantiate(left.ToBoolean() && right.ToBoolean());
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            return new TwoValueComputationStep(ToTweedle(), scope, lhs, rhs, Evaluate, true, false);
        }

        internal override string Operator()
        {
            return "&&";
        }
    }
}