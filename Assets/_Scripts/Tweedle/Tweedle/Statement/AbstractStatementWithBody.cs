using System;
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

        public override void Execute(TweedleFrame frame)
        {
			ExecuteStatement(0, frame);
        }

		private void ExecuteStatement(int index, TweedleFrame frame)
		{
			if (index < statements.Count)
			{
				statements[index].Execute(frame); // TODO Call back with index+1
			}
			else
			{
				frame.Next();
			}
		}
	}
}