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

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			Body.AddSequentialStep(parent, frame);
		}
	}
}