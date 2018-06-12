using System;

namespace Alice.Tweedle
{
	public class ExpressionStatement : TweedleStatement
	{
		TweedleExpression expression;

		public TweedleExpression Expression
		{
			get
			{
				return expression;
			}
		}

		public ExpressionStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			expression.Evaluate(frame, val => next());
		}
	}
}