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
		TweedleArray items;
		bool firstPass = true;

		public EachInArrayNotifyingStep(EachInArrayTogether statement, TweedleFrame frame, NotifyingStep next)
			: base(statement, frame, next)
		{
		}

		internal override void BlockerFinished(NotifyingStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			if (items == null)
			{
				items = (TweedleArray)((NotifyingEvaluationStep)notifyingStep).Result;
			}
		}

		internal override void Execute()
		{
			if (firstPass)
			{
				firstPass = false;
				var frames = items.Values.Select(val => frame.ChildFrame("EachInArrayTogether", statement.ItemVariable, val)).ToList();
				statement.Body.AddParallelSteps(frames, this);
			}
			else
			{
				base.Execute();
			}
		}
	}
}