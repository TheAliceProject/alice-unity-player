using System;
using System.Collections.Generic;
using System.Threading;

namespace Alice.Tweedle
{
	public class BlockStatement
	{
		public List<TweedleStatement> Statements { get; }

		public BlockStatement(List<TweedleStatement> statements)
		{
			Statements = statements;
		}

		public void ExecuteInParallel(TweedleFrame frame, Action next)
		{
			Action allDone = WaitForAll(Statements.Count, next);
			foreach (TweedleStatement statement in Statements)
			{
				statement.Execute(frame.ChildFrame(), allDone);
			}
		}

		Action WaitForAll(int count, Action next)
		{
			int waiting = count;
			return () =>
			{
				Interlocked.Decrement(ref waiting);
				if (waiting == 0)
				{
					next();
				}
			};
		}

		internal void ExecuteInSequence(TweedleFrame frame, Action next)
		{
			ExecuteStatement(0, frame, next);
		}

		void ExecuteStatement(int index, TweedleFrame frame, Action next)
		{
			if (index < Statements.Count)
			{
				Statements[index].Execute(frame, () => ExecuteStatement(index + 1, frame, next));
			}
			else
			{
				next();
			}
		}
	}
}