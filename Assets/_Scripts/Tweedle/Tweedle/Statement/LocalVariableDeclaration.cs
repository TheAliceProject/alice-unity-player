﻿using Alice.VM;

namespace Alice.Tweedle
{
	public class LocalVariableDeclaration : TweedleStatement
	{
		public TweedleLocalVariable Variable { get; }
		public bool IsConstant { get; }

		public LocalVariableDeclaration(bool isConstant, TweedleLocalVariable variable)
		{
			IsConstant = isConstant;
			Variable = variable;
		}

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			Variable.AddInitializerStep(
				new SingleInputActionNotificationStep(
					frame.StackWith(Variable.ToTweedle()),
					frame,
					parent,
					value => frame.SetLocalValue(Variable, value)),
				frame);
		}
	}
}