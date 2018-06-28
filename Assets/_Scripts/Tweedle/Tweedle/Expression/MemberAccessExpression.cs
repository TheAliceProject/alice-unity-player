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

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			return Target.AsStep(parent, frame);
		}
	}
}