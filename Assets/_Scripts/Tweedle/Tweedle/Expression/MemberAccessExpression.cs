using System;
using Alice.VM;

namespace Alice.Tweedle
{
	abstract public class MemberAccessExpression : TweedleExpression
	{
		public TweedleExpression Target { get; }
        internal protected bool invokeSuper;

		public MemberAccessExpression(TweedleExpression target)
			: base(null)
		{
			Target = target;
			invokeSuper = false;
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return Target.AsStep(frame);
		}
	}
}