using System.Collections.Generic;

namespace Alice.Tweedle
{
	abstract public class TweedleAbstractStatementWithBody : TweedleStatement
    {
        private List<TweedleStatement> statements;

        public List<TweedleStatement> Statements
        {
            get
            {
                return statements;
            }
        }

        public TweedleAbstractStatementWithBody(List<TweedleStatement> statements)
		{
            this.statements = statements;
        }
	}
}