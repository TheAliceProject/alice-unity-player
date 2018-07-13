using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ForEachInArrayLoop : AbstractLoop
	{
		internal TweedleLocalVariable item;
		TweedleExpression array;

		public ForEachInArrayLoop(TweedleLocalVariable item, TweedleExpression array, List<TweedleStatement> body)
			: base(body)
		{
			this.item = item;
			this.array = array;
		}

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			return array.AsStep(scope).OnCompletionNotify(new ForEachInArrayStep(this, scope, next));
		}
	}

	internal class ForEachInArrayStep : LoopStep<ForEachInArrayLoop>
	{
		TweedleArray items;
		int index = 0;

		public ForEachInArrayStep(ForEachInArrayLoop statement, ExecutionScope scope, ExecutionStep next)
			: base(statement, scope, next)
		{
		}

		internal override void BlockerFinished(ExecutionStep blockingStep)
		{
			base.BlockerFinished(blockingStep);
			if (items == null)
			{
				items = (TweedleArray)blockingStep.Result;
			}
		}

		internal override void Execute()
		{
			if (index < items.Length)
			{
				var loopScope = scope.ChildScope("ForEach loop", statement.item, items[index++]);
				statement.Body.AddSequentialStep(loopScope, this);
			}
			else
			{
				base.Execute();
			}
		}
	}
}