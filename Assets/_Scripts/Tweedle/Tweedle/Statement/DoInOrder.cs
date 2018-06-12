using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class DoInOrder : TweedleStatement
	{
		BlockStatement body;

		public BlockStatement Body
		{
			get { return body; }
		}

		public DoInOrder(List<TweedleStatement> statements)
		{
			body = new BlockStatement(statements);
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			body.ExecuteInSequence(frame, next);
		}
	}
}