namespace Alice.Tweedle
{
    class LessThanWholeExpression : BinaryNumericExpression<bool, int>
	{
        public LessThanWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.BOOLEAN)
        {
        }

		protected override bool Evaluate(int left, int right)
		{
			return left < right;
		}
	}

	class LessThanDecimalExpression : BinaryNumericExpression<bool, double>
	{
		public LessThanDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.BOOLEAN)
		{
		}

		protected override bool Evaluate(double left, double right)
		{
			return left < right;
		}
	}
}