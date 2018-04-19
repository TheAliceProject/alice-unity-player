using Alice.VM;

namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression(TweedleType type)
			: base(type)
		{
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			return null;
		}
	}
}