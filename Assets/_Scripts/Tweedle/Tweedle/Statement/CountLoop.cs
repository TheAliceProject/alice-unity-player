using System;
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

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new CountLoopStep(this, frame, count.AsStep(frame));
		}
	}

	internal class CountLoopStep : StatementStep<CountLoop>
	{
		EvaluationStep count;
		int index = 0;

		public CountLoopStep(CountLoop statement, TweedleFrame frame, EvaluationStep count)
			: base(statement, frame, count)
		{
			this.count = count;
		}

		internal override bool Execute()
		{
			if (index < count.Result.ToInt())
			{
				var loopFrame = frame.ChildFrame(statement.Variable, TweedleTypes.WHOLE_NUMBER.Instantiate(index));
				AddBlockingStep(statement.Body.ToSequentialStep(loopFrame));
				index++;
				return false;
			}
			return MarkCompleted();
		}
	}
}