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

		internal void AddSequentialStep(TweedleFrame frame, NotifyingStep next)
		{
			frame.vm.AddStep(AsSequentialStep(frame, next));
		}

		internal SequentialSteps AsSequentialStep(TweedleFrame frame, NotifyingStep next)
		{
			return new SequentialSteps(frame, next, this);
		}

		internal SequentialSteps AsSequentialStep(TweedleFrame frame)
		{
			return new SequentialSteps(frame, null, this);
		}

		internal void AddParallelSteps(TweedleFrame frame, NotifyingStep next)
		{
			foreach (TweedleStatement statement in Statements)
			{
				statement.QueueStepToNotify(frame.ChildFrame(), next);
			}
		}

		internal void AddParallelSteps(List<TweedleFrame> frames, NotifyingStep next)
		{
			foreach (TweedleFrame frame in frames)
			{
				AddSequentialStep(frame, next);
			}
		}

		internal class SequentialSteps : NotifyingStep
		{
			protected BlockStatement block;
			int index = 0;

			public SequentialSteps(TweedleFrame frame, NotifyingStep next, BlockStatement block)
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