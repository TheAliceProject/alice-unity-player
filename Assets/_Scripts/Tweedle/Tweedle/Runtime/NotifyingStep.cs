using Alice.Tweedle;

namespace Alice.VM
{
	public class NotifyingStep
	{
		internal NotifyingStep waitingStep;
		int blockerCount = 0;
		StepStatus status;
		internal string callStack;
		protected internal TweedleFrame frame;

		protected internal NotifyingStep(TweedleFrame frame)
			: this(frame, null)
		{
		}

		protected internal NotifyingStep(TweedleFrame frame, NotifyingStep waitingStep)
		{
			if (frame == null)
			{
				throw new System.ArgumentNullException(nameof(frame));
			}

			this.frame = frame;
			this.waitingStep = waitingStep;
			if (waitingStep != null)
			{
				waitingStep.blockerCount++;
			}
		}

		internal void Notify(NotifyingStep parent)
		{
			if (waitingStep == null)
			{
				waitingStep = parent;
				parent.blockerCount++;
			}
			else
			{
				waitingStep.Notify(parent);
			}
		}

		internal void QueueAndNotify(NotifyingStep parent)
		{
			Notify(parent);
			frame.vm.AddStep(this);
		}

		internal bool IsBlocked()
		{
			return status == StepStatus.WaitingOnSteps || status == StepStatus.WaitingOnFrame;
		}

		internal bool IsComplete()
		{
			return status == StepStatus.Conpleted;
		}

		internal void MarkCompleted()
		{
			status = StepStatus.Conpleted;
			if (null != waitingStep)
			{
				waitingStep.BlockerFinished(this);
			}
		}

		internal virtual bool CanRunThisFrame()
		{
			return status == StepStatus.Ready || status == StepStatus.WaitingOnSteps;
		}

		internal virtual void PrepForFrame()
		{
		}

		// Step can do some work and then:
		// * Finish, notifying its waiting step
		// * Add itself back to the queue
		// * Create one or more children to continue work & increment blockerCount
		//  * Add a child step to the queue (single threaded)
		//  * Add multiple child steps to the queue (doTogether)
		internal virtual void Execute()
		{
			// By default finish and notify the waiting step
			MarkCompleted();
		}

		internal virtual void BlockerFinished(NotifyingStep notifyingStep)
		{
			if (--blockerCount == 0)
			{
				status = StepStatus.Ready;
				frame.vm.AddStep(this);
			}
		}

		// Inner steps to complete the execution of this step.
		//internal abstract bool HasNextStepsThisFrame();

		//internal abstract List<NotifyingStep> GetNextSteps();

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
	Conpleted
}