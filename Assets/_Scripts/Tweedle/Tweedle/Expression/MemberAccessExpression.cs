using System;
using Alice.VM;

namespace Alice.Tweedle
{
	abstract public class MemberAccessExpression : TweedleExpression
	{
		public TweedleExpression Target { get; }

		public MemberAccessExpression(TweedleExpression target)
			: base(null)
		{
			Target = target;
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return Target.AsStep(frame);
		}
	}
}