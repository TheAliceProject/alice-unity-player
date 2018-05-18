namespace Alice.Tweedle
{
	class LessThanOrEqualExpression : BinaryNumToBoolExpression
	{
        public LessThanOrEqualExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

		protected override bool Evaluate(int left, int right)
		{
			return left <= right;
		}

		protected override bool Evaluate(double left, double right)
		{
			return left <= right;
		}
	}
}