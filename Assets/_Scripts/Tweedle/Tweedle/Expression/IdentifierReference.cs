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

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			return new NotifyingValueStep("Get Identifier " + Name, frame, frame.GetValue(Name));
		}
	}
}