using System;
using System.Collections.Generic;
using System.Linq;

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

		public override void Execute(TweedleFrame frame, Action next)
		{
			Array.Evaluate(frame, items => ExecuteBody((TweedleArray)items, frame, next));
		}

		void ExecuteBody(TweedleArray items, TweedleFrame frame, Action next)
		{
			var frames = items.Values.Select(val => frame.ChildFrame(ItemVariable, val)).ToList();
			Body.ExecuteFramesInParallel(frames, next);
		}
	}
}