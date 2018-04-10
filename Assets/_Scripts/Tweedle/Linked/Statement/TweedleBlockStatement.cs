using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleBlockStatement : TweedleStatement
	{
		private List<TweedleStatement> statements;

		public TweedleBlockStatement(List<TweedleStatement> statements)
		{
			this.statements = statements;
		}
	}
}
