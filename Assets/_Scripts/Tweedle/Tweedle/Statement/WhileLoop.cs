using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class WhileLoop : AbstractLoop
	{
		public TweedleExpression RunCondition { get; }

		public WhileLoop(TweedleExpression runCondition, List<TweedleStatement> body) : base(body)
		{
			RunCondition = runCondition;
		}

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			return RunCondition.AsStep(frame).OnCompletionNotify(new WhileLoopStep(this, frame, next));
		}
	}

	internal class WhileLoopStep : LoopStep<WhileLoop>
	{
		bool shouldRunBody = false;

		public WhileLoopStep(WhileLoop statement, TweedleFrame frame, ExecutionStep next)
			: base(statement, frame, next)
		{
		}

		internal override void BlockerFinished(ExecutionStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			shouldRunBody = notifyingStep.Result.ToBoolean();
		}

		internal override void Execute()
		{
			if (shouldRunBody)
			{
				var loopFrame = frame.ChildFrame("While loop");
				var shouldRunBodyAgain = statement.RunCondition.AsStep(frame);
				shouldRunBodyAgain.OnCompletionNotify(this);
				statement.Body.AddSequentialStep(loopFrame, shouldRunBodyAgain);
			}
			else
			{
				base.Execute();
			}
		}
	}
}