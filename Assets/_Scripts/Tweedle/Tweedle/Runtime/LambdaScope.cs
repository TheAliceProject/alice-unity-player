using System.Collections.Generic;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	class LambdaScope : InvocationScope
	{
		internal TLambda lambda;

		internal LambdaScope(ExecutionScope caller)
			: base(caller)
		{
		}

		internal override ExecutionStep InvocationStep(string callStackEntry, NamedArgument[] arguments)
		{
			return base.InvocationStep(callStackEntry, arguments);
		}

		internal void QueueInvocationStep(StepSequence sequentialSteps, ITweedleExpression[] arguments, AsyncReturn<TValue> returnVal)
		{
			lambda.AddInvocationSteps(this, sequentialSteps, arguments);
			if (returnVal != null)
			{
				sequentialSteps.AddStep(new DelayedOperationStep(
					"Lambda Completed",
					this,
					() =>
					{
						returnVal.Return(Result);
					}
				));
			}
		}
	}
}
