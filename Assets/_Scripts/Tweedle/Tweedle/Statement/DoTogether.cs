using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class DoTogether : TweedleStatement
	{
		public BlockStatement Body { get; }

		public DoTogether(List<TweedleStatement> statements)
		{
			Body = new BlockStatement(statements);
		}

		internal override void QueueStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			if (IsEnabled)
			{
				Body.AddParallelSteps(frame, next);
			}
		}

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return null;
		}
	}
}