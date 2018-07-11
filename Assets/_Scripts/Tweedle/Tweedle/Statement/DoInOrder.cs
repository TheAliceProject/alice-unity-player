using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class DoInOrder : TweedleStatement
	{
		public BlockStatement Body { get; }

		public DoInOrder(List<TweedleStatement> statements)
		{
			Body = new BlockStatement(statements);
		}

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			return Body.AsSequentialStep(frame, next);
		}
	}
}