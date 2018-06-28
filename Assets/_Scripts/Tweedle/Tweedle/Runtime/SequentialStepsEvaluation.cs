using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	class SequentialStepsEvaluation : NotifyingEvaluationStep
	{
		List<NotifyingStep> steps = new List<NotifyingStep>();
		NotifyingEvaluationStep resultStep;
		int index = 0;

		public SequentialStepsEvaluation(string callStack, NotifyingStep parent)
			: base(parent.frame, parent)
		{
			this.callStack = callStack;
		}

		internal void AddStep(NotifyingStep step)
		{
			steps.Add(step);
		}

		internal void AddEvaluationStep(NotifyingEvaluationStep resultStep)
		{
			steps.Add(resultStep);
			this.resultStep = resultStep;
		}

		internal override void Execute()
		{
			if (index < steps.Count)
			{
				steps[index++].QueueAndNotify(this);
			}
			else
			{
				result = resultStep.Result;
				base.Execute();
			}
		}
	}
}