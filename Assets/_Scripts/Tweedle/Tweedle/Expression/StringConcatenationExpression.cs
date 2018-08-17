namespace Alice.Tweedle
{
	public class StringConcatenationExpression : BinaryExpression
	{

		public StringConcatenationExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs, TweedleTypes.TEXT_STRING)
		{
		}

		protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
		{
			return TweedleTypes.TEXT_STRING.Instantiate(left.ToTextString() + right.ToTextString());
		}

		internal override string Operator()
		{
			return "..";
		}
	}
}