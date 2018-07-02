using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class SequentialStepsEvaluation : NotifyingEvaluationStep
	{
		List<NotifyingStep> steps = new List<NotifyingStep>();
		NotifyingEvaluationStep resultStep;
		int index = 0;

		public SequentialStepsEvaluation(string callStackEntry, TweedleFrame frame)
			: base(frame)
		{
			this.callStack = frame.StackWith(callStackEntry);
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