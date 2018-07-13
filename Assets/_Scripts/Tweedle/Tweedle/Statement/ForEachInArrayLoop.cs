﻿using System.Collections.Generic;
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
			return array.AsStep(frame).OnCompletionNotify(new ForEachInArrayStep(this, frame, next));
		}
	}

	internal class ForEachInArrayStep : LoopStep<ForEachInArrayLoop>
	{
		TweedleArray items;
		int index = 0;

		public ForEachInArrayStep(ForEachInArrayLoop statement, TweedleFrame frame, ExecutionStep next)
			: base(statement, frame, next)
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