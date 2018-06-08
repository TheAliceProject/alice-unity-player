namespace Alice.Tweedle
{
	public abstract class BinaryNumToNumExpression : BinaryExpression
	{
		public BinaryNumToNumExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.NUMBER)
		{
		}

		protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
		{
			if (left.Type is TweedleWholeNumberType && right.Type is TweedleWholeNumberType)
			{
				return TweedleTypes.WHOLE_NUMBER.Instantiate(
					Evaluate(((TweedlePrimitiveValue<int>)left).Value,
							 ((TweedlePrimitiveValue<int>)right).Value));
			}
			return TweedleTypes.DECIMAL_NUMBER.Instantiate(Evaluate(left.ToDouble(), right.ToDouble()));
		}

		protected abstract int Evaluate(int left, int right);

		protected abstract double Evaluate(double left, double right);
	}

	public abstract class BinaryNumToBoolExpression : BinaryExpression
	{
		public BinaryNumToBoolExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.BOOLEAN)
		{
		}

		protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
		{
			if (left.Type is TweedleWholeNumberType && right.Type is TweedleWholeNumberType)
			{
				return TweedleTypes.BOOLEAN.Instantiate(
					Evaluate(((TweedlePrimitiveValue<int>)left).Value,
							 ((TweedlePrimitiveValue<int>)right).Value));
			}
			return TweedleTypes.BOOLEAN.Instantiate(Evaluate(left.ToDouble(), right.ToDouble()));
		}

		protected abstract bool Evaluate(int left, int right);

		protected abstract bool Evaluate(double left, double right);
	}
}