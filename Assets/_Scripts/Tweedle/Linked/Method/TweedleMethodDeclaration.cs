using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleMethodDeclaration : TweedleMethod
	{
		public TweedleMethodDeclaration(List<string> modifiers, TweedleType type, string name, List<TweedleField> required, List<TweedleField> optional, List<TweedleStatement> body) 
			: base(modifiers, type, name, required, optional, body)
		{
		}
	}
}