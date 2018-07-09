using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	class LambdaFrame : InvocationFrame
	{
		internal TweedleLambda lambda;

		internal LambdaFrame(TweedleFrame caller)
			: base(caller)
		{
		}

		internal override NotifyingEvaluationStep InvocationStep(string callStackEntry, Dictionary<string, TweedleExpression> arguments)
		{
			return base.InvocationStep(callStackEntry, arguments);
		}

		internal void QueueInvocationStep(SequentialStepsEvaluation sequentialSteps, List<TweedleExpression> arguments)
		{
			lambda.AddInvocationSteps(this, sequentialSteps, arguments);
		}
	}
}
