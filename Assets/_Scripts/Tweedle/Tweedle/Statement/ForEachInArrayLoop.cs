using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ForEachInArrayLoop : AbstractLoop
	{
		internal TweedleLocalVariable item;
		TweedleExpression array;

		public ForEachInArrayLoop(TweedleLocalVariable item, TweedleExpression array, List<TweedleStatement> body)
			: base(body)
		{
			this.item = item;
			this.array = array;
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			array.Evaluate(frame, items => ExecuteBody(0, (TweedleArray)items, frame, next));
		}

		void ExecuteBody(int index, TweedleArray items, TweedleFrame frame, Action next)
		{
			if (index < items.Length)
			{
				Body.ExecuteInSequence(frame.ChildFrame(item, items[index]),
									   () => ExecuteBody(index + 1, items, frame, next));
			}
			else
			{
				next();
			}
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new ForEachInArrayStep(this, frame, array.AsStep(frame));
		}
	}

	internal class ForEachInArrayStep : StatementStep<ForEachInArrayLoop>
	{
		EvaluationStep array;
		TweedleArray items;
		int index = 0;

		public ForEachInArrayStep(ForEachInArrayLoop statement, TweedleFrame frame, EvaluationStep array)
			: base(statement, frame, array)
		{
			this.array = array;
		}

		internal override bool Execute()
		{
			if (items == null)
			{
				items = (TweedleArray) array.Result;
			}
			if (index < items.Length)
			{
				BlockingSteps.Clear();
				var loopFrame = frame.ChildFrame(statement.item, items[index]);
				AddBlockingStep(statement.Body.ToSequentialStep(loopFrame));
				index++;
				return false;
			}
			return MarkCompleted();
		}
	}
}