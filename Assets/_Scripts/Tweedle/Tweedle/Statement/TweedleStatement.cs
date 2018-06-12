using System;

namespace Alice.Tweedle
{
	abstract public class TweedleStatement
	{
		bool enabled = true;

		public bool IsEnabled
		{
			get
			{
				return enabled;
			}
		}

		abstract public void Execute(TweedleFrame frame, Action next);

		internal void Disable()
		{
			enabled = false;
		}
	}
}