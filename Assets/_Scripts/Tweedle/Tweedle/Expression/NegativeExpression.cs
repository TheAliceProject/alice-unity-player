using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class NegativeExpression : TweedleExpression
	{
		protected TweedleExpression expression;

		internal NegativeExpression(TweedleType type, TweedleExpression expression)
			: base(type)
		{
			this.expression = expression;
		}

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			return expression.AsStep(
				new SingleInputNotificationStep(
					frame.StackWith("-" + expression.ToTweedle()),
					frame,
					parent,
					value => Negate(value)),
				frame);
		}

		internal abstract TweedleValue Negate(TweedleValue value);
	}

	public class NegativeWholeExpression : NegativeExpression
	{

		public NegativeWholeExpression(TweedleExpression expression)
			: base(TweedleTypes.WHOLE_NUMBER, expression)
		{
		}

		internal override TweedleValue Negate(TweedleValue value)
		{
			return TweedleTypes.WHOLE_NUMBER.Instantiate(0 - value.ToInt());
		}
	}

	public class NegativeDecimalExpression : NegativeExpression
	{

		public NegativeDecimalExpression(TweedleExpression expression)
			: base(TweedleTypes.DECIMAL_NUMBER, expression)
		{
		}

		internal override TweedleValue Negate(TweedleValue value)
		{
			return TweedleTypes.DECIMAL_NUMBER.Instantiate(0 - value.ToDouble());
		}
	}
}