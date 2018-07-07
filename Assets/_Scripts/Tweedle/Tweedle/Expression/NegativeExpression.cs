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

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			var val = expression.AsStep(frame);
			val.OnCompletionNotify(new SingleInputNotificationStep("-" + expression.ToTweedle(), frame, Negate));
			return val;
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