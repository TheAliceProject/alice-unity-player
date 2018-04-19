namespace Alice.Tweedle
{
    class MultiplicationWholeExpression : BinaryNumericExpression<int, int>
	{

        public MultiplicationWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.WHOLE_NUMBER)
        {
        }

		protected override int Evaluate(int left, int right)
		{
			return left * right;
		}
	}

	class MultiplicationDecimalExpression : BinaryNumericExpression<double, double>
	{

		public MultiplicationDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.DECIMAL_NUMBER)
		{
		}

		protected override double Evaluate(double left, double right)
		{
			return left * right;
		}
	}
}