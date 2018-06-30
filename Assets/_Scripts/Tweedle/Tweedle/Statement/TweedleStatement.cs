using Alice.VM;

namespace Alice.Tweedle
{
	abstract public class TweedleStatement
	{
		bool enabled = true;

		public bool IsEnabled
		{ get { return enabled; } }

		internal void Disable()
		{
			enabled = false;
		}

		internal virtual void QueueStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			if (enabled)
			{
				frame.vm.AddStep(this.AsStepToNotify(frame, next));
			}
		}

		internal abstract NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next);
	}
}