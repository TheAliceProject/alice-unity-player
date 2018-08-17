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

		internal override void QueueStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			if (IsEnabled)
			{
				Body.AddParallelSteps(scope, next);
			}
		}

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			return null;
		}
	}
}