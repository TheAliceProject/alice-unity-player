namespace Alice.Tweedle
{
	public class SuperExpression : TweedleExpression
	{
		public SuperExpression(TweedleType type)
			: base(type)
		{
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			return null;
		}
	}
}