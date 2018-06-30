using Alice.VM;

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

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return Variable.AsInitializerStep(
				frame,
				new SingleInputActionNotificationStep(
					frame.StackWith(Variable.ToTweedle()),
					frame,
					value => frame.SetLocalValue(Variable, value),
					next));
		}
	}
}