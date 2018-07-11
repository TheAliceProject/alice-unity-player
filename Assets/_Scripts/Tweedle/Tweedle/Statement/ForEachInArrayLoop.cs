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

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			return array.AsStep(frame).OnCompletionNotify(new ForEachInArrayNotifyingStep(this, frame, next));
		}
	}

	internal class ForEachInArrayNotifyingStep : NotifyingStatementStep<ForEachInArrayLoop>
	{
		TweedleArray items;
		int index = 0;

		public ForEachInArrayNotifyingStep(ForEachInArrayLoop statement, TweedleFrame frame, ExecutionStep next)
			: base(statement, frame, new ExecutionStep(frame, next))
		{
		}

		internal override void BlockerFinished(ExecutionStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			if (items == null)
			{
				items = (TweedleArray)notifyingStep.Result;
			}
		}

		internal override void Execute()
		{
			if (index < items.Length)
			{
				var loopFrame = frame.ChildFrame("ForEach loop", statement.item, items[index++]);
				statement.Body.AddSequentialStep(loopFrame, this);
			}
			else
			{
				base.Execute();
			}
		}
	}
}