namespace Alice.Tweedle
{
	class ModuloExpression : BinaryExpression
	{

		public ModuloExpression(ITweedleExpression lhs, ITweedleExpression rhs)
			: base(lhs, rhs, TStaticTypes.WHOLE_NUMBER)
		{
		}

		protected override TValue Evaluate(TValue left, TValue right)
		{
			return TStaticTypes.WHOLE_NUMBER.Instantiate(left.ToInt() % right.ToInt());
		}

		internal override string Operator()
		{
			return "%";
		}
	}
}