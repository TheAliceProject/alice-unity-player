using System;

namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			next(frame.GetThis());
		}
	}
}