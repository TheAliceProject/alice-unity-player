using Alice.VM;

namespace Alice.Tweedle
{
	public class LogicalNotExpression : TweedleExpression
	{
		TweedleExpression expression;

		public LogicalNotExpression(TweedleExpression expression)
			: base(TweedleTypes.BOOLEAN)
		{
			this.expression = expression;
		}

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			var step = expression.AsStep(frame);
			step.OnCompletionNotify(new SingleInputNotificationStep("!" + expression.ToTweedle(), frame, NotPrimitive));
			return step;
		}

		static TweedlePrimitiveValue<bool> NotPrimitive(TweedleValue value)
		{
			return TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean());
		}
	}
}