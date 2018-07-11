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

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			var arrayStep = Array.AsStep(frame);
			var bodyStep = new SingleInputActionNotificationStep(
				"EachInArrayTogether",
				frame,
				items =>
				{
					var frames = ((TweedleArray)items).Values.Select(val => frame.ChildFrame("EachInArrayTogether", ItemVariable, val)).ToList();
					Body.AddParallelSteps(frames, next);
				});
			arrayStep.OnCompletionNotify(bodyStep);
			return arrayStep;
		}
	}
}