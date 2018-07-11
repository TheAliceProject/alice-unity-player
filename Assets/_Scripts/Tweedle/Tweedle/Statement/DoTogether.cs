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

		internal override void QueueStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			if (IsEnabled)
			{
				Body.AddParallelSteps(frame, next);
			}
		}

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			return null;
		}
	}
}