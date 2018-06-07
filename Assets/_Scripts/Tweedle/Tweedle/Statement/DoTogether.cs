using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class DoTogether : TweedleStatement
	{
		BlockStatement body;

		public BlockStatement Body
		{
			get { return body; }
		}

		public DoTogether(List<TweedleStatement> statements)
		{
			body = new BlockStatement(statements);
		}

		public override void Execute(TweedleFrame frame)
		{
			body.ExecuteInParallel(frame);
		}
	}
}