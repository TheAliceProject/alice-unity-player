using Alice.VM;

namespace Alice.Tweedle
{
	public class ExpressionStatement : TweedleStatement
	{
		public TweedleExpression Expression { get; }

		public ExpressionStatement(TweedleExpression expression)
		{
			Expression = expression;
		}

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			return Expression.AsStep(scope).OnCompletionNotify(next);
		}
	}
}