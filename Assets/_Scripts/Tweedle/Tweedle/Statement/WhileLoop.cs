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

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			RunCondition.AddStep(new WhileLoopNotifyingStep(this, frame, parent), frame);
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
				statement.Body.AddSequentialStep(statement.RunCondition.AsStep(this, frame), loopFrame);
			}
			else
			{
				MarkCompleted();
			}
		}
	}
}