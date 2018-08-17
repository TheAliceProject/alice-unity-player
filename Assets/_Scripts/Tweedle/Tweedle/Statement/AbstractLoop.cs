using System.Collections.Generic;

namespace Alice.Tweedle
{
	abstract public class AbstractLoop : TweedleStatement
	{
		public BlockStatement Body { get; }

		public AbstractLoop(List<TweedleStatement> statements)
		{
			Body = new BlockStatement(statements);
		}
	}
}