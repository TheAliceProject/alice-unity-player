using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleField : TweedleValueHolderDeclaration
	{
		List<string> modifiers;

		public List<string> Modifiers
		{
			get { return modifiers; }
		}


		public TweedleField(List<string> modifiers, TweedleType type, string name)
			: base(type, name)
		{
			this.modifiers = modifiers;
		}


		public TweedleField(List<string> modifiers, TweedleType type, string name, TweedleExpression initializer)
			: base(type, name, initializer)
		{
			this.modifiers = modifiers;
		}

		internal ExecutionStep InitializeFieldStep(ConstructorFrame frame, TweedleObject tweedleObject)
		{
			return new SingleInputActionStep(
				InitializerStep(frame),
				value => tweedleObject.Set(Name, value, frame));
		}
	}
}