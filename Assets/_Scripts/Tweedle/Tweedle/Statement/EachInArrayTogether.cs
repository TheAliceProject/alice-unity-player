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

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return Array.AsStep(frame).OnCompletionNotify(new EachInArrayNotifyingStep(this, frame, next));
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