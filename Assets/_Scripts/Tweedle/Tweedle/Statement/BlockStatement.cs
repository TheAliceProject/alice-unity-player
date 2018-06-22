using System.Collections.Generic;
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

		internal ExecutionStep ToSequentialStep(TweedleFrame frame)
		{
			ExecutionStep lastStep = ExecutionStep.NOOP;
			foreach (TweedleStatement stmt in Statements)
            {
				ExecutionStep step = stmt.RootStep(frame);
                step.AddBlockingStep(lastStep);
                lastStep = step;
            }
            return lastStep;
		}

		internal ExecutionStep ToParallelSteps(TweedleFrame frame)
		{
			var blockers = Statements.Select(stmt => stmt.RootStep(frame)).ToList();
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
					AddBlockingStep(block.Statements[index].RootStep(frame));
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
					AddBlockingStep(statement.RootStep(frame));
				}
			}

			internal override bool Execute()
			{
				return MarkCompleted();
			}
		}
	}
}