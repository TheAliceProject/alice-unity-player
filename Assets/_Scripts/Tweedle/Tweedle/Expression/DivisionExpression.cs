namespace Alice.Tweedle
{
    class DivisionExpression : BinaryNumToNumExpression
    {
        public DivisionExpression(ITweedleExpression lhs, ITweedleExpression rhs)
            : base(lhs, rhs)
        {
        }

        protected override int Evaluate(int left, int right)
        {
            return left / right;
        }

        protected override double Evaluate(double left, double right)
        {
            return left / right;
        }

        internal override string Operator()
        {
            return "/";
        }
    }
}