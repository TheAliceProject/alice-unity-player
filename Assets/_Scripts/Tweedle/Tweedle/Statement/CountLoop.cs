using System.Collections.Generic;
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

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			return count.AsStep(frame).OnCompletionNotify(new NotifyingCountLoopStep(this, frame, next));
		}
	}

	class NotifyingCountLoopStep : NotifyingStatementStep<CountLoop>
	{
		int maxCount = -1;
		int index = 0;

		public NotifyingCountLoopStep(CountLoop statement, TweedleFrame frame, ExecutionStep next)
			: base(statement, frame, next)
		{
		}

		internal override void BlockerFinished(ExecutionStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			if (maxCount == -1)
			{
				// Only set this once
				maxCount = notifyingStep.Result.ToInt();
			}
		}

		internal override void Execute()
		{
			if (index < maxCount)
			{
				var loopFrame = frame.ChildFrame("Count loop", statement.Variable, TweedleTypes.WHOLE_NUMBER.Instantiate(index++));
				statement.Body.AddSequentialStep(loopFrame, this);
			}
			else
			{
				base.Execute();
			}
		}
	}
}