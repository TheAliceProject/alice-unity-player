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

		internal void AddSequentialStep(NotifyingStep parent, TweedleFrame frame)
		{
			frame.vm.AddStep(AsSequentialStep(parent, frame));
		}

		internal SequentialSteps AsSequentialStep(NotifyingStep parent, TweedleFrame frame)
		{
			return new SequentialSteps(parent, this, frame);
		}

		internal void AddParallelSteps(NotifyingStep parent, TweedleFrame frame)
		{
			foreach (TweedleStatement statement in Statements)
			{
				statement.AddChildStep(parent, frame.ChildFrame());
			}
		}

		internal void AddParallelSteps(NotifyingStep parent, List<TweedleFrame> frames)
		{
			foreach (TweedleFrame frame in frames)
			{
				AddSequentialStep(parent, frame);
			}
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

		internal class SequentialSteps : NotifyingStep
		{
			protected BlockStatement block;
			int index = 0;

			public SequentialSteps(NotifyingStep parent, BlockStatement block, TweedleFrame frame)
				: base(frame, parent)
			{
				this.block = block;
			}

			internal override void Execute()
			{
				if (index < block.Statements.Count)
				{
					block.Statements[index++].AddChildStep(this, frame);
				}
				else
				{
					MarkCompleted();
				}
			}
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
					AddBlockingStep(block.Statements[index++].RootStep(frame));
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