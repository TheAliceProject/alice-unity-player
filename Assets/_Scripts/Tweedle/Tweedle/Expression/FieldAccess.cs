using Alice.VM;

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

		public FieldAccess(TweedleExpression target, string fieldName)
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

		internal override NotifyingEvaluationStep AsStep(NotifyingStep next, TweedleFrame frame)
		{
			return base.AsStep(
				new SingleInputNotificationStep(
					frame.StackWith("Get Field " + FieldName),
					frame,
					target => target.Get(FieldName),
					next),
				frame);
		}
	}
}