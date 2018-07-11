using Alice.Tweedle;

namespace Alice.VM
{
	public class ExecutionStep
	{
		internal ExecutionStep next;
		int blockerCount = 0;
		StepStatus status;
		internal string callStack;
		protected internal TweedleFrame frame;

		protected TweedleValue result = TweedleNull.NULL;
		public TweedleValue Result { get { return result; } }

		internal TweedleValue EvaluateNow()
		{
			Queue();
			return result;
		}

		protected internal ExecutionStep(TweedleFrame frame)
			: this(frame, null)
		{
		}

		protected internal ExecutionStep(TweedleFrame frame, ExecutionStep next)
		{
			if (frame == null)
			{
				throw new System.ArgumentNullException(nameof(frame));
			}

			this.frame = frame;
			this.next = next;
			if (next != null)
			{
				next.blockerCount++;
			}
		}

		internal ExecutionStep OnCompletionNotify(ExecutionStep finalNext)
		{
			if (next == null)
			{
				next = finalNext;
				next.blockerCount++;
			}
			else
			{
				next.OnCompletionNotify(finalNext);
			}
			return this;
		}

		internal void Queue()
		{
			frame.vm.AddStep(this);
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

		internal virtual void BlockerFinished(ExecutionStep notifyingStep)
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

enum StepStatus
{
	Ready,
	WaitingOnSteps,
	WaitingOnFrame,
	Completed
}