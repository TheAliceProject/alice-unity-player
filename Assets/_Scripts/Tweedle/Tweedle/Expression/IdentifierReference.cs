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
			// stack? frame.StackWith("Get Identifier " + Name)
			return new NotifyingValueStep(frame, parent, frame.GetValue(Name));
		}
	}
}