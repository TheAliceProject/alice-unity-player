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

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			//frame.GetThis().GetClass().GetSuper();
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			throw new NotImplementedException();
		}
	}
}