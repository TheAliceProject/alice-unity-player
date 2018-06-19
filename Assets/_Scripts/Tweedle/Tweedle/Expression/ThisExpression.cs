using System;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new ValueStep(frame.GetThis());
		}
	}
}