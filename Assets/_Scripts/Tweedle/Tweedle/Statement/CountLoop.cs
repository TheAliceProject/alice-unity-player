using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class CountLoop : AbstractLoop
	{
		public TweedleLocalVariable Variable { get; }

		TweedleExpression count;

		public CountLoop(string variableName, TweedleExpression count, List<TweedleStatement> body) : base(body)
		{
			Variable = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, variableName);
			this.count = count;
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			count.Evaluate(frame, value => ExecuteBody(0, ((TweedlePrimitiveValue<int>)value).Value, frame, next));
		}

		void ExecuteBody(int index, int limit, TweedleFrame frame, Action next)
		{
			if (index < limit)
			{
				var loopFrame = frame.ChildFrame();
				loopFrame.SetLocalValue(Variable, TweedleTypes.WHOLE_NUMBER.Instantiate(index));
				Body.ExecuteInSequence(loopFrame,
									   () => ExecuteBody(index + 1, limit, frame, next));
			}
			else
			{
				next();
			}
		}
	}
}