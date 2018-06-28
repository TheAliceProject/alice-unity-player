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

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			return expression.AsStep(
				new SingleInputNotificationStep(
					frame.StackWith("!" + expression.ToTweedle()),
					frame,
					parent,
					value => TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean())),
				frame);
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new SingleInputStep(frame.StackWith("!" + expression.ToTweedle()),
				expression.AsStep(frame),
				value => TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean()));
		}
	}
}