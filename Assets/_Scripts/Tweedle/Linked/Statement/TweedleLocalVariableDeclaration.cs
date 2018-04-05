using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleLocalVariableDeclaration : TweedleStatement
	{
		private List<TweedleField> variables;

		public TweedleLocalVariableDeclaration(List<TweedleField> variables)
		{
			this.variables = variables;
		}
	}
}