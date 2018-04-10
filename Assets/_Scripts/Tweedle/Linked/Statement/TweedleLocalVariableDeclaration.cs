using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleLocalVariableDeclaration : TweedleStatement
	{
		private List<TweedleField> field;

		public TweedleLocalVariableDeclaration(List<TweedleField> field)
		{
			this.field = field;
		}
	}
}