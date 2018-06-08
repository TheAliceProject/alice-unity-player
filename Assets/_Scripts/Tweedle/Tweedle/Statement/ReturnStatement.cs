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

		public override void Execute(TweedleFrame frame)
		{
			// evaluate expression, adding next step for it to call
			expression.Evaluate(frame.ExecutionFrame(
				val =>
				{
					// pop frames until a method frame
					TweedleFrame caller = frame.PopMethod();
					// invoke next action with the value of the expression
					caller.Next(val);
				}));
		}
	}
}