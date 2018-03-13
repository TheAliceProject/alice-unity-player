using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleMethod
    {
		private string name;
		private List<TweedleStatement> instructions;

		public TweedleMethod(string name, List<TweedleStatement> instructions)
		{
			this.name = name;
			this.instructions = instructions;
		}
    }
}