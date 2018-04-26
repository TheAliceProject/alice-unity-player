using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class CountLoop : AbstractLoop
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

        public CountLoop(string variableName, TweedleExpression count, List<TweedleStatement> body) : base(body)
        {
            this.variable = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, variableName);
			this.count = count;
		}
	}
}