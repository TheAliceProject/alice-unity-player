namespace Alice.Tweedle
{
	public class AdditionWholeExpression : BinaryNumericExpression<int, int>
    {
        public AdditionWholeExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.WHOLE_NUMBER)
        {
        }

		protected override int Evaluate(int left, int right)
		{
			return left + right;
		}
	}

	public class AdditionDecimalExpression : BinaryNumericExpression<double, double>
	{
		public AdditionDecimalExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.DECIMAL_NUMBER)
		{
		}

		protected override double Evaluate(double left, double right)
		{
			return left + right;
		}
	}
}