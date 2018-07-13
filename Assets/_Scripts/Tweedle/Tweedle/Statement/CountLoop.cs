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
			return count.AsStep(frame).OnCompletionNotify(new CountLoopStep(this, frame, next));
		}
	}

	class CountLoopStep : LoopStep<CountLoop>
	{
		int maxCount = -1;
		int index = 0;

		public CountLoopStep(CountLoop statement, TweedleFrame frame, ExecutionStep next)
			: base(statement, frame, next)
		{
		}

		internal override void BlockerFinished(ExecutionStep blockingStep)
		{
			base.BlockerFinished(blockingStep);
			if (maxCount == -1)
			{
				// Only set this once
				maxCount = blockingStep.Result.ToInt();
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