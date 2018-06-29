using Alice.VM;

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

		internal override void AddStep(NotifyingStep next, TweedleFrame frame)
		{
			expression.AddStep(
				new SingleInputActionNotificationStep(
					frame.StackWith("return " + expression.ToTweedle()),
					frame,
					result => ((MethodFrame)frame).Return(result),
					next),
				frame);
		}
	}
}