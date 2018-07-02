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

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return RunCondition.AsStep(frame).Notify(new WhileLoopNotifyingStep(this, frame, next));
		}
	}

	internal class WhileLoopNotifyingStep : NotifyingStatementStep<WhileLoop>
	{
		bool shouldRunBody = false;

		public WhileLoopNotifyingStep(WhileLoop statement, TweedleFrame frame, NotifyingStep parent)
			: base(statement, frame, parent)
		{
		}

		internal override void BlockerFinished(NotifyingStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			shouldRunBody = ((NotifyingEvaluationStep)notifyingStep).Result.ToBoolean();
		}

		internal override void Execute()
		{
			if (shouldRunBody)
			{
				var loopFrame = frame.ChildFrame("While loop");
				var shouldRunBodyAgain = statement.RunCondition.AsStep(frame);
				shouldRunBodyAgain.Notify(this);
				statement.Body.AddSequentialStep(shouldRunBodyAgain, loopFrame);
			}
			else
			{
				MarkCompleted();
			}
		}
	}
}