namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			return null;
		}
	}
}