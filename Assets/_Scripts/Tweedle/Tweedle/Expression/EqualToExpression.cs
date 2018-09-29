namespace Alice.Tweedle
{
	class EqualToExpression : BinaryExpression
	{

		public EqualToExpression(ITweedleExpression lhs, ITweedleExpression rhs)
			: base(lhs, rhs, TBuiltInTypes.BOOLEAN)
		{
		}

		protected override TValue Evaluate(TValue left, TValue right)
		{
			return TBuiltInTypes.BOOLEAN.Instantiate(left.Equals(right));
		}

		internal override string Operator()
		{
			return "=";
		}
	}
}