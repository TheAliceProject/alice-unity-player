using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class BinaryNumericExpression<T, R>  : BinaryExpression
	{
		private TweedlePrimitiveType<T> primitiveType;

		public BinaryNumericExpression(TweedleExpression lhs, TweedleExpression rhs, TweedlePrimitiveType<T> type)
			: base(lhs, rhs, type)
		{
			primitiveType = type;
		}

		protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
		{
			return primitiveType.Instantiate(Evaluate(ValueCast(left), ValueCast(right)));
		}

		private R ValueCast(TweedleValue val)
		{
			return ((TweedlePrimitiveValue<R>)val).Value;
		}

		protected abstract T Evaluate(R left, R right);
	}
}