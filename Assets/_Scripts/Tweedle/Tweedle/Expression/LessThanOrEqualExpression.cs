namespace Alice.Tweedle
{
    class LessThanOrEqualWholeExpression : BinaryNumericExpression<bool, int>
	{
        public LessThanOrEqualWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.BOOLEAN)
        {
        }

		protected override bool Evaluate(int left, int right)
		{
			return left <= right;
		}
	}

	class LessThanOrEqualDecimalExpression : BinaryNumericExpression<bool, double>
	{
		public LessThanOrEqualDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.BOOLEAN)
		{
		}

		protected override bool Evaluate(double left, double right)
		{
			return left <= right;
		}
	}
}