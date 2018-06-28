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

		internal void AddChildStep(NotifyingStep parent, TweedleFrame frame)
		{
			if (enabled)
			{
				AddStep(parent, frame);
			}
		}

		internal abstract void AddStep(NotifyingStep parent, TweedleFrame frame);
	}
}