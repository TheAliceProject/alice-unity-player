using System;
using System.Collections.Generic;
using System.Threading;
using Alice.VM;
using System.Linq;

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

		internal void ExecuteFramesInParallel(List<TweedleFrame> frames, Action next)
		{
			Action allDone = WaitForAll(frames.Count, next);
			foreach (TweedleFrame frame in frames)
			{
				ExecuteInSequence(frame, allDone);
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

		internal ExecutionStep ToSequentialStep(TweedleFrame frame)
		{
			return new SequentialStepsStep(this, frame);
		}

		internal ExecutionStep ToParallelSteps(TweedleFrame frame)
		{
			var blockers = Statements.Select(stmt => stmt.AsStep(frame)).ToList();
			return new ParallelStepsStep(blockers);
		}

		internal ExecutionStep ExecuteFramesInParallel(List<TweedleFrame> frames)
		{
			List<ExecutionStep> blockers = frames.Select(frame => new SequentialStepsStep(this, frame)).ToList<ExecutionStep>();
			return new ParallelStepsStep(blockers);
		}

		internal class SequentialStepsStep : ExecutionStep
		{
			protected BlockStatement block;
			protected TweedleFrame frame;
			int index = 0;

			public SequentialStepsStep(BlockStatement block, TweedleFrame frame)
			{
				this.block = block;
				this.frame = frame;
			}

			internal override bool Execute()
			{
				if (index < block.Statements.Count)
				{
					AddBlockingStep(block.Statements[index].Execute(frame));
					index++;
					return false;
				}
				return MarkCompleted();
			}
		}

		internal class ParallelStepsStep : ExecutionStep
		{
			public ParallelStepsStep(List<ExecutionStep> blockers)
				: base(blockers)
			{
			}

			public ParallelStepsStep(BlockStatement block, TweedleFrame frame)
			{
				foreach (TweedleStatement statement in block.Statements)
				{
					AddBlockingStep(statement.AsStep(frame));
				}
			}

			internal override bool Execute()
			{
				return MarkCompleted();
			}
		}
	}
}