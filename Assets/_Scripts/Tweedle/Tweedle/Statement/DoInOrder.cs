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

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return Body.AsSequentialStep(frame, next);
		}
	}
}