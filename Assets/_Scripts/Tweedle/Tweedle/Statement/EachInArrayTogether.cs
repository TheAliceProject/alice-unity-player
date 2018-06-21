using System;
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
}