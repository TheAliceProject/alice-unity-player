﻿using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class CountLoop : AbstractLoop
	{
		public TweedleLocalVariable Variable { get; }

		TweedleExpression count;

		public CountLoop(string variableName, TweedleExpression count, List<TweedleStatement> body) : base(body)
		{
			Variable = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, variableName);
			this.count = count;
		}

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			count.AddStep(new NotifyingCountLoopStep(this, frame, parent), frame);
		}
	}

	class NotifyingCountLoopStep : NotifyingStatementStep<CountLoop>
	{
		int maxCount = -1;
		int index = 0;

		public NotifyingCountLoopStep(CountLoop statement, TweedleFrame frame, NotifyingStep parent)
			: base(statement, frame, parent)
		{
		}

		internal override void BlockerFinished(NotifyingStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			if (maxCount == -1)
			{
				// Only set this once
				maxCount = ((NotifyingEvaluationStep)notifyingStep).Result.ToInt();
			}
		}

		internal override void Execute()
		{
			if (index < maxCount)
			{
				var loopFrame = frame.ChildFrame("Count loop", statement.Variable, TweedleTypes.WHOLE_NUMBER.Instantiate(index));
				statement.Body.AddSequentialStep(this, loopFrame);
				index++;
			}
			else
			{
				base.Execute();
			}
		}
	}
}