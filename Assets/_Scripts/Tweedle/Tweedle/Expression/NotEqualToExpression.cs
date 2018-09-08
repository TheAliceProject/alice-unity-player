namespace Alice.Tweedle
{
	class NotEqualToExpression : BinaryExpression
	{

		public NotEqualToExpression(ITweedleExpression lhs, ITweedleExpression rhs)
			: base(lhs, rhs, TStaticTypes.BOOLEAN)
		{
		}

		protected override TValue Evaluate(TValue left, TValue right)
		{
			return TStaticTypes.BOOLEAN.Instantiate(!left.Equals(right));
		}

		internal override string Operator()
		{
			return "!=";
		}
	}
}