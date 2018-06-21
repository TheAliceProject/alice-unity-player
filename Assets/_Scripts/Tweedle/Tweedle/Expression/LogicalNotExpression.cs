using System;
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

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new SingleInputStep(frame.StackWith("!" + expression.ToTweedle()),
				expression.AsStep(frame),
				value => TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean()));
		}
	}
}