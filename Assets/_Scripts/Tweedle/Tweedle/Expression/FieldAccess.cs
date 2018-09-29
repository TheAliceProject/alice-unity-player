using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public class FieldAccess : MemberAccessExpression
	{
		public string FieldName { get; }

		public FieldAccess(string fieldName)
			: base(new ThisExpression())
		{
			FieldName = fieldName;
		}

		public FieldAccess(ITweedleExpression target, string fieldName)
			: base(target)
		{
			FieldName = fieldName;
		}

		public static FieldAccess Super(string fieldName)
		{
			FieldAccess access = new FieldAccess(fieldName);
			access.invokeSuper = true;
			return access;
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			ExecutionStep targetStep = TargetStep(scope);
			targetStep.OnCompletionNotify(
				new ValueComputationStep(
					"Get Field ",
					scope,
					target => target.Get(scope, FieldName)));
			return targetStep;
		}

		public override string ToTweedle()
		{
            return Target.ToTweedle() + "." + FieldName;
        }
	}
}