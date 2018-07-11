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

		internal virtual void QueueStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			if (enabled)
			{
				AsStepToNotify(frame, next).Queue();
			}
		}

		internal abstract ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next);
	}
}