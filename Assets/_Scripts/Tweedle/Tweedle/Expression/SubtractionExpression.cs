namespace Alice.Tweedle
{
    class SubtractionWholeExpression : BinaryNumericExpression<int, int>
	{

        public SubtractionWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.WHOLE_NUMBER)
        {
        }

		protected override int Evaluate(int left, int right)
		{
			return left - right;
		}
	}

	class SubtractionDecimalExpression : BinaryNumericExpression<double, double>
	{

		public SubtractionDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.DECIMAL_NUMBER)
		{
		}

		protected override double Evaluate(double left, double right)
		{
			return left - right;
		}
	}
}