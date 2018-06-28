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

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			Body.AddParallelSteps(parent, frame);
		}
	}
}