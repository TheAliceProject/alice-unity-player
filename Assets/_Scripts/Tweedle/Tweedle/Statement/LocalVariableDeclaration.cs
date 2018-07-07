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
			var initStep = Variable.AsInitializerStep(frame);
			var storeStep = new SingleInputActionNotificationStep(
				Variable.ToTweedle(),
				frame,
				value => frame.SetLocalValue(Variable, value));
			initStep.OnCompletionNotify(storeStep);
			storeStep.OnCompletionNotify(next);
			return initStep;
		}
	}
}