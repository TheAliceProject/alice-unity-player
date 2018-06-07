using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class WhileLoop : AbstractLoop
	{
		TweedleExpression runCondition;

		public TweedleExpression RunCondition
		{
			get { return runCondition; }
		}

		public WhileLoop(TweedleExpression runCondition, List<TweedleStatement> body) : base(body)
		{
			this.runCondition = runCondition;
		}
	}
}