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

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return Expression.AsStep(frame);
		}
	}
}