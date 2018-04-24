using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleConditionalStatement : TweedleStatement
	{
        private TweedleExpression condition;

        public TweedleExpression Condition
        {
            get
            {
                return condition;
            }
        }

        private List<TweedleStatement> thenBody;

        public List<TweedleStatement> ThenBody
        {
            get
            {
                return thenBody;
            }
        }

        private List<TweedleStatement> elseBody;

        public List<TweedleStatement> ElseBody
        {
            get
            {
                return elseBody;
            }
        }

        public TweedleConditionalStatement(TweedleExpression condition, List<TweedleStatement> thenBody, List<TweedleStatement> elseBody)
		{
            this.condition = condition;
            this.thenBody = thenBody;
			this.elseBody = elseBody;
		}
	}
}