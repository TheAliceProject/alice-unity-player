using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class SimpleStep : EvaluationStep
	{
		Func<TweedleValue> body;

		public SimpleStep(Func<TweedleValue> body)
		{
			this.body = body;
		}

		public SimpleStep(ExecutionStep blockingStep, Func<TweedleValue> body)
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