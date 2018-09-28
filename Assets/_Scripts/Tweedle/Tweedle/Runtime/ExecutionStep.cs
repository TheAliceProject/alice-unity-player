using Alice.Tweedle;

namespace Alice.Tweedle.VM
{
	public class ExecutionStep
	{
		#region Types

		protected enum StepStatus
		{
			Ready,
			WaitingOnSteps,
			WaitingOnFrame,
			Completed
		}

		#endregion // Types

		internal ExecutionStep next;
		int blockerCount = 0;
		protected StepStatus status;
		internal string callStack;
		protected internal ExecutionScope scope;

		protected TValue result = TValue.NULL;
		public TValue Result { get { return result; } }

		public TValue EvaluateNow()
		{
			Queue();
			return result;
		}

		protected internal ExecutionStep(ExecutionScope scope)
			: this(scope, null)
		{
		}

		protected internal ExecutionStep(ExecutionScope scope, ExecutionStep next)
		{
			if (scope == null)
			{
				throw new System.ArgumentNullException(nameof(scope));
			}

			this.scope = scope;
			this.next = next;
			if (next != null)
			{
				next.blockerCount++;
			}
		}

		public ExecutionStep OnCompletionNotify(ExecutionStep finalNext)
		{
			if (next == null)
			{
				next = finalNext;
				next.blockerCount++;
			}
			else
			{
				// Add at the end of the chain of steps.
				next.OnCompletionNotify(finalNext);
			}
			// Return earliest step in the chain
			return this;
		}

		internal void Queue()
		{
			scope.vm.AddStep(this);
		}

		internal void QueueAndNotify(ExecutionStep finalNext)
		{
			OnCompletionNotify(finalNext);
			Queue();
		}

		internal bool IsBlocked()
		{
			return status == StepStatus.WaitingOnSteps || status == StepStatus.WaitingOnFrame;
		}

		internal bool IsComplete()
		{
			return status == StepStatus.Completed;
		}

		internal virtual bool CanRunThisFrame()
		{
			return status == StepStatus.Ready || status == StepStatus.WaitingOnSteps;
		}

		internal virtual void PrepForFrame()
		{
			status = StepStatus.Ready;
		}

		// Step can do some work and then:
		// * Finish and notify its next step, by invoking this base method
		// * Add itself back to the queue
		// * Create one or more children to do work & increment blockerCount
		//  * Add a child step to the queue (single threaded)
		//  * Add multiple child steps to the queue (doTogether)
		internal virtual void Execute()
		{
			// Each step should eventually finish and notify the next step
			status = StepStatus.Completed;
			if (null != next)
			{
				next.BlockerFinished(this);
			}
		}

		internal virtual void BlockerFinished(ExecutionStep blockingStep)
		{
			if (--blockerCount == 0)
			{
				status = StepStatus.Ready;
				Queue();
			}
		}

		internal string CallStack()
		{
			return callStack;
		}
	}
}