using System;
using System.Collections.Generic;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class DoubleInputStep : EvaluationStep
	{
		EvaluationStep blockingValueA;
		EvaluationStep blockingValueB;
		Func<TweedleValue, TweedleValue, TweedleValue> body;

		public DoubleInputStep(EvaluationStep blockingValueA,
							   EvaluationStep blockingValueB,
							   Func<TweedleValue, TweedleValue, TweedleValue> body)
			: base(new List<ExecutionStep> { blockingValueA, blockingValueB })
		{
			this.body = body;
			this.blockingValueA = blockingValueA;
			this.blockingValueB = blockingValueB;
		}

		internal override bool Execute()
		{
			result = body.Invoke(blockingValueA.Result, blockingValueB.Result);
			return MarkCompleted();
		}
	}
}