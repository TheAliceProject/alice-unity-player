using Alice.Tweedle.VM;

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

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			return new ValueStep("Get Identifier " + Name, scope, scope.GetValue(Name));
		}
	}
}