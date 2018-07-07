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

		internal NotifyingStep InitializeField(TweedleFrame frame, TweedleObject tweedleObject)
		{
			return Initializer
				.AsStep(frame)
				.OnCompletionNotify(new SingleInputActionNotificationStep("", frame, value => tweedleObject.Set(Name, value, frame)));
		}
	}
}