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

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return Expression.AsStep(frame).Notify(next);
		}
	}
}