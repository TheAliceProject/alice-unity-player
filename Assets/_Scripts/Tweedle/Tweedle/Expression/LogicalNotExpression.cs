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

		internal override NotifyingEvaluationStep AsStep(NotifyingStep next, TweedleFrame frame)
		{
			return expression.AsStep(
				new SingleInputNotificationStep(
					frame.StackWith("!" + expression.ToTweedle()),
					frame,
					value => TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean()),
					next),
				frame);
		}
	}
}