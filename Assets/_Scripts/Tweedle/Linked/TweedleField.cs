using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleField
    {
		public string Name
		{
			get { return name; }
		}

		public TweedleType Type
		{
			get { return type; }
		}

		public List<string> Modifiers
		{
			get { return modifiers; }
		}

		private string name;
		private TweedleType type;
		private List<string> modifiers;
		private TweedleStatement initializer;

		public TweedleField(TweedleType type, string name)
		{
			this.name = name;
			this.type = type;
		}

		public TweedleField(List<string> modifiers, TweedleType type, string name)
		{
			this.modifiers = modifiers;
			this.type = type;
			this.name = name;
		}

		public TweedleField(TweedleType type, string name, TweedleStatement initializer) 
		{
			this.type = type;
			this.name = name;
			this.initializer = initializer;
		}

		public TweedleField(List<string> modifiers, TweedleType type, string name, TweedleStatement initializer)
		{
			this.modifiers = modifiers;
			this.type = type;
			this.name = name;
			this.initializer = initializer;
		}

		public void Initializer(TweedleExpression expr)
		{

		}
	}
}