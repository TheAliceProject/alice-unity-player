namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		public override void Evaluate(TweedleFrame frame)
		{
			frame.Next(frame.GetThis());
		}
	}
}