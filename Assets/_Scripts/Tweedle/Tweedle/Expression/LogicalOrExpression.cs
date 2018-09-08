namespace Alice.Tweedle
{
	class LogicalOrExpression : BinaryExpression
	{

		public LogicalOrExpression(ITweedleExpression lhs, ITweedleExpression rhs)
			: base(lhs, rhs, TStaticTypes.BOOLEAN)
		{
		}

		protected override TValue Evaluate(TValue left, TValue right)
		{
			return TStaticTypes.BOOLEAN.Instantiate(left.ToBoolean() || right.ToBoolean());
		}

		internal override string Operator()
		{
			return "||";
		}
	}
}