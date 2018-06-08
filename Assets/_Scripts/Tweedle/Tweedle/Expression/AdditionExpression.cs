namespace Alice.Tweedle
{
	public class AdditionExpression : BinaryNumToNumExpression
    {
        public AdditionExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override int Evaluate(int left, int right)
		{
			return left + right;
		}

        protected override double Evaluate(double left, double right)
		{
            return left + right;
		}
	}
}