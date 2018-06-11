using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class BlockStatement
	{
		public List<TweedleStatement> Statements { get; }

		public BlockStatement(List<TweedleStatement> statements)
		{
			Statements = statements;
		}

		public void ExecuteInSequence(TweedleFrame frame)
		{
			ExecuteStatement(0, frame);
		}

		void ExecuteStatement(int index, TweedleFrame frame)
		{
			if (index < Statements.Count)
			{
				Statements[index].Execute(frame.ExecutionFrame(
					val => ExecuteStatement(index + 1, frame)));
			}
			else
			{
				frame.Next();
			}
		}

		public void ExecuteInParallel(TweedleFrame frame)
		{
			TweedleFrame allDone = frame.ParallelFrame(Statements.Count);
			foreach (TweedleStatement statement in Statements)
			{
				statement.Execute(allDone);
			}
		}
	}
}