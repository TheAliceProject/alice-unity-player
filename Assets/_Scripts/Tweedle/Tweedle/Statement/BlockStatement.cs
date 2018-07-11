using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class BlockStatement
	{
		public List<TweedleStatement> Statements { get; }

		public BlockStatement(List<TweedleStatement> statements)
		{
			Statements = statements;
		}

		internal void AddSequentialStep(TweedleFrame frame, ExecutionStep next)
		{
			AsSequentialStep(frame, next).Queue();
		}

		internal SequentialSteps AsSequentialStep(TweedleFrame frame, ExecutionStep next)
		{
			return new SequentialSteps(frame, next, this);
		}

		internal SequentialSteps AsSequentialStep(TweedleFrame frame)
		{
			return new SequentialSteps(frame, null, this);
		}

		internal void AddParallelSteps(TweedleFrame frame, ExecutionStep next)
		{
			foreach (TweedleStatement statement in Statements)
			{
				statement.QueueStepToNotify(frame.ChildFrame(), next);
			}
		}

		internal void AddParallelSteps(List<TweedleFrame> frames, ExecutionStep next)
		{
			foreach (TweedleFrame frame in frames)
			{
				AddSequentialStep(frame, next);
			}
		}

		internal class SequentialSteps : ExecutionStep
		{
			protected BlockStatement block;
			int index = 0;

			public SequentialSteps(TweedleFrame frame, ExecutionStep next, BlockStatement block)
				: base(frame, next)
			{
				this.block = block;
			}

			internal override void Execute()
			{
				if (index < block.Statements.Count)
				{
					block.Statements[index++].QueueStepToNotify(frame, this);
				}
				else
				{
					base.Execute();
				}
			}
		}
	}
}