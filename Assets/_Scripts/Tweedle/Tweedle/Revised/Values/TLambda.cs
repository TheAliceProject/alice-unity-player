using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public sealed class TLambda
	{
		// readonly LambdaExpression source;

		// public TLambda(LambdaExpression expression)
		// {
		// 	source = expression;
		// }

		// internal void AddInvocationSteps(LambdaScope scope, StepSequence main, ITweedleExpression[] arguments)
		// {
		// 	AddArgumentSteps(scope, main, arguments);
		// 	main.AddStep(source.Body.AsSequentialStep(scope));
		// 	main.AddStep(ResultStep(scope));
		// }

		// void AddArgumentSteps(LambdaScope scope, StepSequence main, ITweedleExpression[] arguments)
		// {
		// 	for (int i = 0; i < arguments.Length; i++)
		// 	{
		// 		TParameter param = source.Parameters[i];
		// 		ITweedleExpression argExp = arguments[i];
		// 		ExecutionStep argStep = argExp.AsStep(scope.callingScope);
		// 		var storeStep = new ValueComputationStep(
		// 			"Arg",
		// 			scope.callingScope,
		// 			argVal => scope.SetLocalValue(param, argVal));
		// 		argStep.OnCompletionNotify(storeStep);
		// 		main.AddStep(argStep);
		// 	}
		// }

		// ExecutionStep ResultStep(LambdaScope scope)
		// {
		// 	return new ValueComputationStep("call", scope, arg => scope.Result);
		// }
	}
}