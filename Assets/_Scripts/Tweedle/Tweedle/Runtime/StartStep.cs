using System;
using Alice.VM;

namespace Alice.Tweedle
{
	internal class StartStep : EvaluationStep
	{
		bool started = false;
		Func<EvaluationStep> generator;
		EvaluationStep child;

		public StartStep(string callStack, Func<EvaluationStep> generator)
		{
			this.generator = generator;
			this.callStack = callStack;
		}

		public StartStep(ExecutionStep blockingStep, Func<EvaluationStep> generator)
			: base(blockingStep)
		{
			this.generator = generator;
		}

		internal override bool Execute()
		{
			if (!started)
			{
				started = true;
				child = generator.Invoke();
				AddBlockingStep(child);
				return false;
			}
			else
			{
				result = child.Result;
				return MarkCompleted();
			}
		}
	}
}