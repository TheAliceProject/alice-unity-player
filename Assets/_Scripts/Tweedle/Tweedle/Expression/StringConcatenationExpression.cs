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
			string l = ((TweedlePrimitiveValue<string>)left).Value;
			string r = ((TweedlePrimitiveValue<string>)right).Value;
			return TweedleTypes.TEXT_STRING.Instantiate(l + r);
		}
	}
}