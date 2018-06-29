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
			return new NotifyingValueStep(frame.StackWith("Get Identifier " + Name), frame, parent, frame.GetValue(Name));
		}
	}
}