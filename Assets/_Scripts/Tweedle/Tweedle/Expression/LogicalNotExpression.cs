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

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			var step = expression.AsStep(scope);
			step.OnCompletionNotify(new ValueComputationStep("!" + expression.ToTweedle(), scope, NotPrimitive));
			return step;
		}

		static TweedlePrimitiveValue<bool> NotPrimitive(TweedleValue value)
		{
			return TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean());
		}
	}
}