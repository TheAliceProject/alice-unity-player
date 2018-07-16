using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleLambda : TweedleValue
	{
		readonly LambdaExpression source;

		public TweedleLambda(LambdaExpression expression)
			: base(expression.LambdaType())
		{
			source = expression;
		}

		internal void AddInvocationSteps(LambdaScope scope, StepSequence main, List<TweedleExpression> arguments)
		{
			AddArgumentSteps(scope, main, arguments);
			main.AddStep(source.Body.AsSequentialStep(scope));
			main.AddStep(ResultStep(scope));
		}

		void AddArgumentSteps(LambdaScope scope, StepSequence main, List<TweedleExpression> arguments)
		{
			for (int i = 0; i < arguments.Count; i++)
			{
				TweedleRequiredParameter param = source.Parameters[i];
				TweedleExpression argExp = arguments[i];
				ExecutionStep argStep = argExp.AsStep(scope.callingScope);
				var storeStep = new ValueComputationStep(
					"Arg",
					scope.callingScope,
					argVal => scope.SetLocalValue(param, argVal));
				argStep.OnCompletionNotify(storeStep);
				main.AddStep(argStep);
			}
		}

		ExecutionStep ResultStep(LambdaScope scope)
		{
			return new ValueComputationStep("call", scope, arg => scope.Result);
		}
	}
}