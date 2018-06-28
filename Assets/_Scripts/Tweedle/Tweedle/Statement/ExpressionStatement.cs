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

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			Expression.AddStep(parent, frame);
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return Expression.AsStep(frame);
		}
	}
}