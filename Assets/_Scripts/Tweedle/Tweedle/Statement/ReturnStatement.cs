using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class ReturnStatement : TweedleStatement
    {
        ITweedleExpression expression;

        public ITweedleExpression Expression
        {
            get { return expression; }
        }

        public TTypeRef Type
        {
            get { return expression.Type; }
        }

        public ReturnStatement()
        {
            expression = TValue.UNDEFINED;
        }

        public ReturnStatement(ITweedleExpression expression)
        {
            this.expression = expression;
        }

        internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
        {
            var valStep = expression.AsStep(scope);
            var returnStep = new ValueOperationStep(
                "return " + expression.ToTweedle(),
                scope,
                result => scope.Return(result));
            valStep.OnCompletionNotify(returnStep);
            returnStep.OnCompletionNotify(next);
            return valStep;
        }
    }
}