using System;

namespace Alice.Tweedle
{
	public class ReturnStatement : TweedleStatement
	{
		TweedleExpression expression;

		public TweedleExpression Expression
		{
			get { return expression; }
		}

		public TweedleType Type
		{
			get { return expression.Type; }
		}

		public ReturnStatement()
		{
			expression = TweedleNull.NULL;
		}

		public ReturnStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			// Ignores next since method is finished
			expression.Evaluate(frame, val => ((InvocationFrame)frame).Complete(val));
		}
	}
}