using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleProperty : TweedleField
	{
		public TweedleProperty(TweedleType type, string name) : base(type, name)
		{
		}

		public TweedleProperty(List<string> modifier, TweedleType type, string name) : base(modifier, type, name)
		{
		}

		public TweedleProperty(TweedleType type, string name, TweedleStatement initializer) : base(type, name, initializer)
		{
		}

		public TweedleProperty(List<string> modifier, TweedleType type, string name, TweedleStatement initializer) : base(modifier, type, name, initializer)
		{
		}
	}
}
