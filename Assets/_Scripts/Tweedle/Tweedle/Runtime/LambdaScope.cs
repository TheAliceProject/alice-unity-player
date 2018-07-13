using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	class LambdaScope : InvocationScope
	{
		internal TweedleLambda lambda;

		internal LambdaScope(ExecutionScope caller)
			: base(caller)
		{
		}

		internal override ExecutionStep InvocationStep(string callStackEntry, Dictionary<string, TweedleExpression> arguments)
		{
			return base.InvocationStep(callStackEntry, arguments);
		}

		internal void QueueInvocationStep(StepSequence sequentialSteps, List<TweedleExpression> arguments)
		{
			lambda.AddInvocationSteps(this, sequentialSteps, arguments);
		}
	}
}
