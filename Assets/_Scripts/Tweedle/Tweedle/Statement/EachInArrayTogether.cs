using System.Collections.Generic;
using System.Linq;
using Alice.VM;

namespace Alice.Tweedle
{
	public class EachInArrayTogether : TweedleStatement
	{
		public TweedleLocalVariable ItemVariable { get; }
		public TweedleExpression Array { get; }
		public BlockStatement Body { get; }

		public EachInArrayTogether(TweedleLocalVariable itemVariable, TweedleExpression array, List<TweedleStatement> statements)
		{
			ItemVariable = itemVariable;
			Array = array;
			Body = new BlockStatement(statements);
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new EachInArrayStep(this, frame, Array.AsStep(frame));
		}

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			Array.AddStep(new EachInArrayNotifyingStep(this, frame, parent), frame);
		}
	}

	internal class EachInArrayStep : StatementStep<EachInArrayTogether>
	{
		EvaluationStep array;
		bool started = false;

		public EachInArrayStep(EachInArrayTogether statement, TweedleFrame frame, EvaluationStep array)
			: base(statement, frame, array)
		{
			this.array = array;
		}

		internal override bool Execute()
		{
			if (started)
			{
				return MarkCompleted();
			}
			started = true;
			var frames = ((TweedleArray)array.Result).Values.Select(val => frame.ChildFrame("EachInArrayTogether", statement.ItemVariable, val)).ToList();
			AddBlockingStep(statement.Body.ExecuteFramesInParallel(frames));
			return false;
		}
	}

	internal class EachInArrayNotifyingStep : NotifyingStatementStep<EachInArrayTogether>
	{
		NotifyingStep arrayStep;

		public EachInArrayNotifyingStep(EachInArrayTogether statement, TweedleFrame frame, NotifyingStep parent)
			: base(statement, frame, new NotifyingStep(frame, parent))
		{
		}
		internal override void BlockerFinished(NotifyingStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			if (arrayStep == null)
			{
				arrayStep = notifyingStep;
			}
		}

		internal override void Execute()
		{
			var frames = ((TweedleArray)((NotifyingEvaluationStep)arrayStep).Result).Values.Select(val => frame.ChildFrame("EachInArrayTogether", statement.ItemVariable, val)).ToList();
			statement.Body.AddParallelSteps(this.waitingStep, frames);
		}
	}
}