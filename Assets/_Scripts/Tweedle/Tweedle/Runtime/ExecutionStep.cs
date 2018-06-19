using System;
using System.Collections.Generic;
using System.Linq;
using Alice.Tweedle;

namespace Alice.VM
{
	abstract class ExecutionStep
	{
		public static ExecutionStep NOOP = new NoOpStep();

		internal List<ExecutionStep> BlockingSteps { get; }
		bool changed = true;
		bool completed = false;

		protected ExecutionStep()
		{
			BlockingSteps = new List<ExecutionStep>();
		}

		protected ExecutionStep(ExecutionStep blockingStep)
		{
			BlockingSteps = new List<ExecutionStep>() { blockingStep };
		}

		protected ExecutionStep(List<ExecutionStep> blockingSteps)
		{
			BlockingSteps = blockingSteps;
		}

		protected void AddBlockingSteps(List<ExecutionStep> steps)
		{
			BlockingSteps.AddRange(steps);
			Changed();
		}

		private void Changed()
		{
			changed = true;
		}

		internal List<ExecutionStep> IncompleteBlockingSteps()
		{
			return BlockingSteps.Where(step => !step.IsComplete()).ToList();
		}

		internal void AddBlockingStep(ExecutionStep step)
		{
			if (BlockingSteps.Count > 100)
			{
				UnityEngine.Debug.Log("Too many blocking tasks");
				throw new TweedleRuntimeException("Too many tasks for now.");
			}
			BlockingSteps.Add(step);
			Changed();
		}

		internal bool IsBlocked()
		{
			foreach (var blocker in BlockingSteps)
			{
				if (!blocker.IsComplete())
				{
					return true;
				}
			}
			return false;
		}

		internal bool IsChanged()
		{
			return changed;
		}

		internal bool MarkQueued()
		{
			return changed = false;
		}

		internal bool IsComplete()
		{
			return completed;
		}

		internal bool MarkCompleted()
		{
			return completed = true;
		}

		internal virtual bool CanRunThisFrame()
		{
			return true;
		}

		internal virtual void PrepForFrame()
		{
		}

		// Returns itself with a tree of blocking steps for further work, or null if done
		// Return true if complete, false if it should be run again next frame
		// TODO check for blocking here and prevent improper execution
		internal abstract bool Execute();
	}

	internal class NoOpStep : ExecutionStep
	{
		internal NoOpStep()
		{
			MarkCompleted();
		}

		internal override bool Execute()
		{
			return true;
		}
	}

	internal class CompletionStep : ExecutionStep
	{
		internal CompletionStep()
		{
		}

		protected CompletionStep(ExecutionStep blockingStep)
			:base(blockingStep)
        {
        }

		internal override bool Execute()
		{
			return MarkCompleted();
		}
	}
}