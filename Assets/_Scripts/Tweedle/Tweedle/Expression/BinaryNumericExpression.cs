namespace Alice.Tweedle
{
	public abstract class BinaryNumToNumExpression : BinaryExpression
	{
		public BinaryNumToNumExpression(ITweedleExpression lhs, ITweedleExpression rhs)
			: base(lhs, rhs, TStaticTypes.NUMBER)
		{
		}

		protected override TValue Evaluate(TValue left, TValue right)
		{
			if (left.Type == TStaticTypes.WHOLE_NUMBER && right.Type == TStaticTypes.WHOLE_NUMBER)
			{
                return TStaticTypes.WHOLE_NUMBER.Instantiate(Evaluate(left.ToInt(), right.ToInt()));
            }
			return TStaticTypes.DECIMAL_NUMBER.Instantiate(Evaluate(left.ToDouble(), right.ToDouble()));
		}

		protected abstract int Evaluate(int left, int right);

		protected abstract double Evaluate(double left, double right);
	}

	public abstract class BinaryNumToBoolExpression : BinaryExpression
	{
		public BinaryNumToBoolExpression(ITweedleExpression lhs, ITweedleExpression rhs)
			: base(lhs, rhs, TStaticTypes.BOOLEAN)
		{
		}

		protected override TValue Evaluate(TValue left, TValue right)
		{
            if (left.Type == TStaticTypes.WHOLE_NUMBER && right.Type == TStaticTypes.WHOLE_NUMBER)
            {
                return TStaticTypes.BOOLEAN.Instantiate(Evaluate(left.ToInt(), right.ToInt()));
            }
            return TStaticTypes.BOOLEAN.Instantiate(Evaluate(left.ToDouble(), right.ToDouble()));
		}

		protected abstract bool Evaluate(int left, int right);

		protected abstract bool Evaluate(double left, double right);
	}
}