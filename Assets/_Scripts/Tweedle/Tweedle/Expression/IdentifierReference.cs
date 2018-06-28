using Alice.VM;

namespace Alice.Tweedle
{
	public class IdentifierReference : TweedleExpression
	{
		public string Name { get; }

		public IdentifierReference(string name)
			: base(null)
		{
			Name = name;
		}

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			return new NotifyingValueStep(frame, parent, frame.GetValue(Name));
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new ContextEvaluationStep(frame.StackWith("Get Identifier " + Name), () => frame.GetValue(Name));
		}
	}
}