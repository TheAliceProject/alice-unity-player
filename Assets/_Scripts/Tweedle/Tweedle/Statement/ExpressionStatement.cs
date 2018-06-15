using System;
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

		public override void Execute(TweedleFrame frame, Action next)
		{
			Expression.Evaluate(frame, val => next());
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return Expression.AsStep(frame);
		}
	}
}