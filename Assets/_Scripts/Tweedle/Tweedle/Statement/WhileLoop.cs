using System;
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

		public override void Execute(TweedleFrame frame, Action next)
		{
			RunCondition.Evaluate(frame, value =>
			{
				if (((TweedlePrimitiveValue<bool>)value).Value)
					Body.ExecuteInSequence(frame.ChildFrame(), () => Execute(frame, next));
				else
					next();
			});
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
				AddBlockingStep(statement.Body.ToSequentialStep(frame.ChildFrame()));
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
}