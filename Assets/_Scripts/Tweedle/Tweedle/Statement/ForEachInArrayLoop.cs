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

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return array.AsStep(frame).Notify(new ForEachInArrayNotifyingStep(this, frame, next));
		}
	}

	internal class ForEachInArrayNotifyingStep : NotifyingStatementStep<ForEachInArrayLoop>
	{
		TweedleArray items;
		int index = 0;

		public ForEachInArrayNotifyingStep(ForEachInArrayLoop statement, TweedleFrame frame, NotifyingStep parent)
			: base(statement, frame, new NotifyingStep(frame, parent))
		{
		}

		internal override void BlockerFinished(NotifyingStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			if (items == null)
			{
				items = (TweedleArray)((NotifyingEvaluationStep)notifyingStep).Result;
			}
		}

		internal override void Execute()
		{
			if (index < items.Length)
			{
				var loopFrame = frame.ChildFrame("ForEach loop", statement.item, items[index++]);
				statement.Body.AddSequentialStep(this, loopFrame);
			}
			else
			{
				MarkCompleted();
			}
		}
	}
}