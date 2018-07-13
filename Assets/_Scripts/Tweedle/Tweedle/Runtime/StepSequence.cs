using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class StepSequence : ExecutionStep
	{
		List<ExecutionStep> steps = new List<ExecutionStep>();
		int index = 0;

		public StepSequence(string callStackEntry, ExecutionScope scope)
			: base(scope)
		{
			this.callStack = scope.StackWith(callStackEntry);
		}

		internal void AddStep(ExecutionStep step)
		{
			steps.Add(step);
		}

		internal override void Execute()
		{
			// UnityEngine.Debug.Log("  Running steps " + steps);
			if (index < steps.Count)
			{
				// UnityEngine.Debug.Log(">>Queueing step " + index + " - " + steps[index].callStack + " " + steps[index] + "<<");
				steps[index++].QueueAndNotify(this);
			}
			else
			{
				result = steps.Count > 0 ? steps[index - 1].Result : TweedleNull.NULL;
				// UnityEngine.Debug.Log(">>Reporting result " + result + "<<");
				base.Execute();
			}
		}
	}
}