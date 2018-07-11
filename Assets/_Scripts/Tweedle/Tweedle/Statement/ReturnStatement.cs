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

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			var valStep = expression.AsStep(frame);
			var returnStep = new SingleInputActionNotificationStep(
				"return " + expression.ToTweedle(),
				frame,
				result => ((InvocationFrame)frame).Return(result));
			valStep.OnCompletionNotify(returnStep);
			returnStep.OnCompletionNotify(next);
			return valStep;
		}
	}
}