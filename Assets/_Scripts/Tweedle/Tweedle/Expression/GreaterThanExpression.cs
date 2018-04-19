namespace Alice.Tweedle
{
    class GreaterThanWholeExpression : BinaryNumericExpression<bool, int>
	{
        public GreaterThanWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.BOOLEAN)
        {
        }

		protected override bool Evaluate(int left, int right)
		{
			return left > right;
		}
	}

	class GreaterThanDecimalExpression : BinaryNumericExpression<bool, double>
	{
		public GreaterThanDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.BOOLEAN)
		{
		}

		protected override bool Evaluate(double left, double right)
		{
			return left > right;
		}
	}
}