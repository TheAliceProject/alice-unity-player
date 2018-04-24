using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleWhileLoop : TweedleAbstractLoop
	{
		private TweedleExpression runCondition;

        public TweedleExpression RunCondition
        {
            get
            {
                return runCondition;
            }
        }

        public TweedleWhileLoop(TweedleExpression runCondition, List<TweedleStatement> body) : base(body)
		{
            this.runCondition = runCondition;
		}
	}
}