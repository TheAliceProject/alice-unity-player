using System.Collections.Generic;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public sealed class TLambda
	{
        private readonly LambdaExpression source;
        private readonly ExecutionScope creationScope;

        public TLambda(LambdaExpression expression, ExecutionScope creationScope)
		{
			source = expression;
            this.creationScope = creationScope;
        }

		internal void AddInvocationSteps(LambdaScope scope, StepSequence main, ITweedleExpression[] arguments)
		{
			AddArgumentSteps(scope, main, arguments);
			main.AddStep(source.Body.AsSequentialStep(scope));
			main.AddStep(ResultStep(scope));
		}

		void AddArgumentSteps(LambdaScope scope, StepSequence main, ITweedleExpression[] arguments)
		{
			for (int i = 0; i < arguments.Length; i++)
			{
				TParameter param = source.Parameters[i];
				ITweedleExpression argExp = arguments[i];
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

		// TODO(Alex): Remove, replace with some other mechanism
		public VirtualMachine GetOwner()
		{
            return creationScope.vm;
        }

		public LambdaEvaluation Evaluation(ITweedleExpression[] inArguments)
		{
            TValue thisVal = ((TLambdaType)this.source.Type.Get()).Instantiate(this);

            if (inArguments != null && inArguments.Length > 0)
            {
                return new LambdaEvaluation(thisVal, inArguments);
            }
			else
			{
				return new LambdaEvaluation(thisVal);
			}
		}
	}
}