using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class DoTogether : AbstractStatementWithBody
	{
        public DoTogether(List<TweedleStatement> body) : base(body)
		{
		}

		public override void Execute(TweedleFrame frame)
		{
			TweedleFrame allDone = frame.ParallelFrame(Statements.Count);
			foreach (TweedleStatement statement in Statements)
			{
				statement.Execute(allDone);
			}
		}
	}
}