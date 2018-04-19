namespace Alice.Tweedle
{
    class GreaterThanOrEqualWholeExpression : BinaryNumericExpression<bool, int>
	{
        public GreaterThanOrEqualWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.BOOLEAN)
        {
        }

		protected override bool Evaluate(int left, int right)
		{
			return left >= right;
		}
	}

	class GreaterThanOrEqualDecimalExpression : BinaryNumericExpression<bool, double>
	{
		public GreaterThanOrEqualDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.BOOLEAN)
		{
		}

		protected override bool Evaluate(double left, double right)
		{
			return left >= right;
		}
	}
}