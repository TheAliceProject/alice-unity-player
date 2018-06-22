using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class ContextEvaluationStep : EvaluationStep
	{
		Func<TweedleValue> body;

		public ContextEvaluationStep(string callStack, Func<TweedleValue> body)
		{
			this.body = body;
			this.callStack = callStack;
		}

		public ContextEvaluationStep(string callStack, ExecutionStep blockingStep, Func<TweedleValue> body)
			: base(blockingStep)
		{
			this.body = body;
			this.callStack = callStack;
		}

		internal override bool Execute()
		{
			result = body.Invoke();
			return MarkCompleted();
		}
	}
}