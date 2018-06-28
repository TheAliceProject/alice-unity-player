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
	}
}