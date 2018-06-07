using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class BlockStatement
	{
		List<TweedleStatement> statements;

		public List<TweedleStatement> Statements
		{
			get { return statements; }
		}

		public BlockStatement(List<TweedleStatement> statements)
		{
			this.statements = statements;
		}

		public void ExecuteInSequence(TweedleFrame frame)
		{
			ExecuteStatement(0, frame);
		}

		void ExecuteStatement(int index, TweedleFrame frame)
		{
			if (index < statements.Count)
			{
				statements[index].Execute(frame); // TODO Call back with index+1
			}
			else
			{
				frame.Next();
			}
		}

		public void ExecuteInParallel(TweedleFrame frame)
		{
			TweedleFrame allDone = frame.ParallelFrame(statements.Count);
			foreach (TweedleStatement statement in statements)
			{
				statement.Execute(allDone);
			}
		}
	}
}