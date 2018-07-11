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

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			return Expression.AsStep(frame).OnCompletionNotify(next);
		}
	}
}