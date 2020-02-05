using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class NegativeExpression : UnaryExpression
    {
        protected ITweedleExpression expression;

        internal NegativeExpression(ITweedleExpression expression)
            : base(TBuiltInTypes.NUMBER)
        {
            this.expression = expression;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            var val = expression.AsStep(scope);
            val.OnCompletionNotify(new ValueComputationStep("-" + expression.ToTweedle(), scope, Negate));
            return val;
        }

        public override TValue EvaluateLiteral()
        {
            return Negate((TValue)expression);
        }

        private TValue Negate(TValue value)
        {
            if (value.Type == TBuiltInTypes.WHOLE_NUMBER)
            {
                return TBuiltInTypes.WHOLE_NUMBER.Instantiate(-value.ToInt());
            }
            else
            {
                return TBuiltInTypes.DECIMAL_NUMBER.Instantiate(-value.ToDouble());
            }
        }

        public override string ToTweedle()
        {
            return "-" + expression.ToTweedle();
        }
    }

}