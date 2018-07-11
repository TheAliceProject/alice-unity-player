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

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new ValueStep("Get Identifier " + Name, frame, frame.GetValue(Name));
		}
	}
}