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

		internal NotifyingStep InitializeField(ConstructorFrame frame, TweedleObject tweedleObject)
		{
			return Initializer.AsStep(
				new SingleInputActionNotificationStep("", frame, null, value => tweedleObject.Set(Name, value, frame)),
				frame);
		}
	}
}