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
	}

	public class NegativeWholeExpression : NegativeExpression
	{

		public NegativeWholeExpression(TweedleExpression expression) 
			: base(TweedleTypes.WHOLE_NUMBER, expression)
		{
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			TweedlePrimitiveValue<int> primitive = (TweedlePrimitiveValue<int>)expression.Evaluate(frame);
			return TweedleTypes.WHOLE_NUMBER.Instantiate(0 - primitive.Value);
		}
	}

	public class NegativeDecimalExpression : NegativeExpression
	{

		public NegativeDecimalExpression(TweedleExpression expression)
			: base(TweedleTypes.DECIMAL_NUMBER, expression)
		{
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			TweedlePrimitiveValue<double> primitive = (TweedlePrimitiveValue<double>)expression.Evaluate(frame);
			return TweedleTypes.DECIMAL_NUMBER.Instantiate(0 - primitive.Value);
		}
	}
}