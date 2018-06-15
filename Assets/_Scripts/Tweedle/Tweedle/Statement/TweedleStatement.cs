using System;
using System.Collections.Generic;
using Alice.VM;

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

		internal void Disable()
		{
			enabled = false;
		}

		abstract public void Execute(TweedleFrame frame, Action next);

		internal ExecutionStep Execute(TweedleFrame frame)
		{
			if (enabled)
			{
				return AsStep(frame);
			}
			else
			{
				return ExecutionStep.NOOP;
			}
		}

		internal abstract ExecutionStep AsStep(TweedleFrame frame);
	}
}