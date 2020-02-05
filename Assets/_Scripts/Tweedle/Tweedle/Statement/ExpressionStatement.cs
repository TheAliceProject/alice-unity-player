using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class ExpressionStatement : TweedleStatement
    {
        public ITweedleExpression Expression { get; }

        public ExpressionStatement(ITweedleExpression expression)
        {
            Expression = expression;
        }

        internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
        {
            return Expression.AsStep(scope).OnCompletionNotify(next);
        }

        public override string ToString()
        {
            return Expression.ToTweedle();
        }
    }
}