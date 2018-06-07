using System.Collections.Generic;

namespace Alice.Tweedle
{
	abstract public class AbstractLoop : TweedleStatement
	{
		BlockStatement body;

		public BlockStatement Body
		{
			get { return body; }
		}

		public AbstractLoop(List<TweedleStatement> statements)
		{
			body = new BlockStatement(statements);
		}

		public override void Execute(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}