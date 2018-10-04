using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class LogicalNotExpression : UnaryExpression
    {
        ITweedleExpression expression;

        public LogicalNotExpression(ITweedleExpression expression)
            : base(TBuiltInTypes.BOOLEAN)
        {
            this.expression = expression;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            var step = expression.AsStep(scope);
            step.OnCompletionNotify(new ValueComputationStep("!" + expression.ToTweedle(), scope, NotPrimitive));
            return step;
        }

        static TValue NotPrimitive(TValue value)
        {
            return TBuiltInTypes.BOOLEAN.Instantiate(!value.ToBoolean());
        }

        public override TValue EvaluateLiteral()
        {
            return NotPrimitive((TValue)expression);
        }
    }
}