using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleCountLoop : TweedleAbstractLoop
	{
        private TweedleLocalVariable variable;

        public TweedleLocalVariable Variable
        {
            get
            {
                return variable;
            }
        }

        private TweedleExpression count;

        public TweedleCountLoop(string variableName, TweedleExpression count, List<TweedleStatement> body) : base(body)
        {
            this.variable = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, variableName);
			this.count = count;
		}
	}
}