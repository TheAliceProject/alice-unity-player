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

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			var initStep = Variable.AsInitializerStep(scope);
			var storeStep = new OperationStep(
				Variable.ToTweedle(),
				scope,
				value => scope.SetLocalValue(Variable, value));
			initStep.OnCompletionNotify(storeStep);
			storeStep.OnCompletionNotify(next);
			return initStep;
		}
	}
}