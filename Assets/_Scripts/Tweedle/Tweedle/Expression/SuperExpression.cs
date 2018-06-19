using System;
using Alice.VM;

namespace Alice.Tweedle
{
	public class SuperExpression : TweedleExpression
	{
		public SuperExpression(TweedleType type)
			: base(type)
		{
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			//frame.GetThis().GetClass().GetSuper();
			throw new NotImplementedException();
		}
	}
}