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

		internal ExecutionStep TargetStep(ExecutionScope scope)
		{
			return Target.AsStep(scope);
		}
	}
}