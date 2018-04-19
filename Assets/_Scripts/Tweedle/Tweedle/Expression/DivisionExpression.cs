namespace Alice.Tweedle
{
    class DivisionWholeExpression : BinaryNumericExpression<int, int>
	{
        public DivisionWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.WHOLE_NUMBER)
        {
        }

		protected override int Evaluate(int left, int right)
		{
			return left / right;
		}
	}

	class DivisionDecimalExpression : BinaryNumericExpression<double, double>
	{
		public DivisionDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.DECIMAL_NUMBER)
		{
		}

		protected override double Evaluate(double left, double right)
		{
			return left / right;
		}
	}
}