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

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new WhileLoopStep(this, frame, RunCondition.AsStep(frame));
		}
	}

	internal class WhileLoopStep : StatementStep<WhileLoop>
	{
		EvaluationStep condition;

		public WhileLoopStep(WhileLoop statement, TweedleFrame frame, EvaluationStep condition)
			: base(statement, frame, condition)
		{
			this.condition = condition;
		}

		internal override bool Execute()
		{
			if (BlockingSteps.Contains(condition))
			{
				return ExecuteBodyOrFinish();
			}
			else
			{
				return RerunCondition();
			}
		}

		private bool ExecuteBodyOrFinish()
		{
			if (condition.Result.ToBoolean())
			{
				BlockingSteps.Clear();
				AddBlockingStep(statement.Body.ToSequentialStep(frame.ChildFrame("While loop")));
				return false;
			}
			return MarkCompleted();
		}

		private bool RerunCondition()
		{
			BlockingSteps.Clear();
			condition = statement.RunCondition.AsStep(frame);
			AddBlockingStep(condition);
			return false;
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