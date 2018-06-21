using Alice.VM;

namespace Alice.Tweedle
{
	public class FieldAccess : MemberAccessExpression
	{
		public string FieldName { get; }

		public FieldAccess(TweedleExpression target, string fieldName)
			: base(target)
		{
			FieldName = fieldName;
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new SingleInputStep(
				base.AsStep(frame),
				target => target.Get(FieldName));
		}
	}
}