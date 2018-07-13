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

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			var initStep = Variable.AsInitializerStep(frame);
			var storeStep = new OperationStep(
				Variable.ToTweedle(),
				frame,
				value => frame.SetLocalValue(Variable, value));
			initStep.OnCompletionNotify(storeStep);
			storeStep.OnCompletionNotify(next);
			return initStep;
		}
	}
}