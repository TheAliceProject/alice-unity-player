using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class ConditionalStatement : TweedleStatement
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

        public ConditionalStatement(TweedleExpression condition, List<TweedleStatement> thenBody, List<TweedleStatement> elseBody)
		{
            this.condition = condition;
            this.thenBody = thenBody;
			this.elseBody = elseBody;
		}

		public override void Execute(TweedleFrame frame)
		{
			condition.Evaluate(frame.ExecutionFrame(value => ExecuteBody(value, frame)));
		}

		private void ExecuteBody(TweedleValue value, TweedleFrame frame)
		{
			if (value.ToBoolean())
			{
				//thenBody.Execute(frame);
			}
			else
			{
				//elseBody.Execute(frame);
			}
		}
	}
}