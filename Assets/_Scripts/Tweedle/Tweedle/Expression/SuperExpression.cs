namespace Alice.Tweedle
{
	public class SuperExpression : TweedleExpression
	{
		public SuperExpression(TweedleType type)
			: base(type)
		{
		}

		public override void Evaluate(TweedleFrame frame)
		{
			//frame.GetThis().GetClass().GetSuper();
		}
	}
}