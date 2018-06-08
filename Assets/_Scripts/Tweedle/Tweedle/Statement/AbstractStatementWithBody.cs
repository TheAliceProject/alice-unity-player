using System.Collections.Generic;

namespace Alice.Tweedle
{
	abstract public class AbstractStatementWithBody : TweedleStatement
    {
        private List<TweedleStatement> statements;

        public List<TweedleStatement> Statements
        {
            get
            {
                return statements;
            }
        }

        public AbstractStatementWithBody(List<TweedleStatement> statements)
		{
            this.statements = statements;
        }
	}
}