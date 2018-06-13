using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class ForEachInArrayLoop : AbstractLoop
	{
		TweedleLocalVariable item;
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
	}
}