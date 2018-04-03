using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleField
    {
		private string name;
		private TweedleType type;
		private List<string> modifier;
		private TweedleStatement initializer;

		public TweedleField(TweedleType type, string name)
		{
			this.name = name;
			this.type = type;
		}

		public TweedleField(TweedleType type, string name, List<string> modifier)
		{
			this.name = name;
			this.type = type;
			this.modifier = modifier;
		}

		public TweedleField(TweedleType type, string name, TweedleStatement initializer) 
		{
			this.name = name;
			this.type = type;
			this.initializer = initializer;
		}

		public TweedleField(TweedleType type, List<string> modifier, string name, TweedleStatement initializer)
		{
			this.name = name;
			this.type = type;
			this.modifier = modifier;
			this.initializer = initializer;
		}
	}
}