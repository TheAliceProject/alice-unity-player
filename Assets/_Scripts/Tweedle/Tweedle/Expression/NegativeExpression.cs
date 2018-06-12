using System;

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
	}

	public class NegativeWholeExpression : NegativeExpression
	{

		public NegativeWholeExpression(TweedleExpression expression)
			: base(TweedleTypes.WHOLE_NUMBER, expression)
		{
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			expression.Evaluate(frame,
				value => next(TweedleTypes.WHOLE_NUMBER.Instantiate(0 - value.ToInt())));
		}
	}

	public class NegativeDecimalExpression : NegativeExpression
	{

		public NegativeDecimalExpression(TweedleExpression expression)
			: base(TweedleTypes.DECIMAL_NUMBER, expression)
		{
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			expression.Evaluate(frame,
				value => next(TweedleTypes.DECIMAL_NUMBER.Instantiate(0 - value.ToDouble())));
		}
	}
}