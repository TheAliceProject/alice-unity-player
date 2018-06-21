using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class ContextEvaluationStep : EvaluationStep
	{
		Func<TweedleValue> body;

		public ContextEvaluationStep(Func<TweedleValue> body)
		{
			this.body = body;
		}

		public ContextEvaluationStep(ExecutionStep blockingStep, Func<TweedleValue> body)
			: base(blockingStep)
		{
			this.body = body;
		}

		internal override bool Execute()
		{
			result = body.Invoke();
			return MarkCompleted();
		}
	}
}