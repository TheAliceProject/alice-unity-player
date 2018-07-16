using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleField : TweedleValueHolderDeclaration
	{
		public List<string> Modifiers { get; }

		public TweedleField(List<string> modifiers, TweedleType type, string name)
			: base(type, name)
		{
			Modifiers = modifiers;
		}

		public TweedleField(List<string> modifiers, TweedleType type, string name, TweedleExpression initializer)
			: base(type, name, initializer)
		{
			Modifiers = modifiers;
		}

		internal ExecutionStep InitializeField(ExecutionScope scope, TweedleObject tweedleObject)
		{
			return Initializer
				.AsStep(scope)
				.OnCompletionNotify(new ValueOperationStep("", scope, value => tweedleObject.Set(Name, value, scope)));
		}
	}
}