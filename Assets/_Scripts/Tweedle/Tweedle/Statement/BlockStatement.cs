using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class BlockStatement
	{
		public TweedleStatement[] Statements { get; }

		public BlockStatement(TweedleStatement[] statements)
		{
			Statements = statements;
		}

		internal void AddSequentialStep(ExecutionScope scope, ExecutionStep next)
		{
			AsSequentialStep(scope, next).Queue();
		}

		internal SequentialSteps AsSequentialStep(ExecutionScope scope, ExecutionStep next)
		{
			return new SequentialSteps(scope, next, this);
		}

		internal SequentialSteps AsSequentialStep(ExecutionScope scope)
		{
			return new SequentialSteps(scope, null, this);
		}

		internal void AddParallelSteps(ExecutionScope scope, ExecutionStep next)
		{
			foreach (TweedleStatement statement in Statements)
			{
				statement.QueueStepToNotify(scope.ChildScope(), next);
			}
		}

		internal void AddParallelSteps(List<ExecutionScope> scopes, ExecutionStep next)
		{
			foreach (ExecutionScope scope in scopes)
			{
				AddSequentialStep(scope, next);
			}
		}

		internal class SequentialSteps : ExecutionStep
		{
			protected BlockStatement block;
			int index = 0;

			public SequentialSteps(ExecutionScope scope, ExecutionStep next, BlockStatement block)
				: base(scope, next)
			{
				this.block = block;
			}

			internal override void Execute()
			{
				if (index < block.Statements.Length)
				{
					block.Statements[index++].QueueStepToNotify(scope, this);
				}
				else
				{
					base.Execute();
				}
			}
		}
	}
}