namespace Alice.Tweedle
{
	class LogicalAndExpression : BinaryExpression
	{

		public LogicalAndExpression(ITweedleExpression lhs, ITweedleExpression rhs)
			: base(lhs, rhs, TStaticTypes.BOOLEAN)
		{
		}

		protected override TValue Evaluate(TValue left, TValue right)
		{
			return TStaticTypes.BOOLEAN.Instantiate(left.ToBoolean() && right.ToBoolean());
		}

		internal override string Operator()
		{
			return "&&";
		}
	}
}