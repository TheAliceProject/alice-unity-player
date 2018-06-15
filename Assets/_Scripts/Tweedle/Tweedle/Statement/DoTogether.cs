using System;
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

		public override void Execute(TweedleFrame frame, Action next)
		{
			Body.ExecuteInParallel(frame, next);
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return Body.ToParallelSteps(frame);
		}
	}
}