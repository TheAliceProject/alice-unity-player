using System;
using System.Collections.Generic;

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
					Body.ExecuteInSequence(frame, () => Execute(frame, next));
				else
					next();
			});
		}
	}
}