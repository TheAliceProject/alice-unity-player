using System.Collections.Generic;

namespace Alice.Tweedle
{
    abstract public class TweedleMethod
    {
		public string Name
		{
			get { return name; }
		}

		private List<string> modifiers;
		private TweedleType type;
		private string name;
		private List<TweedleField> required;
		private List<TweedleField> optional;
		private List<TweedleStatement> body;

		public TweedleMethod(TweedleType type, string name, List<TweedleField> required, List<TweedleField> optional, List<TweedleStatement> body)
		{
			this.type = type;
			this.name = name;
			this.required = required;
			this.optional = optional;
			this.body = body;
		}

		public TweedleMethod(List<string> modifiers, TweedleType type, string name, List<TweedleField> required, List<TweedleField> optional, List<TweedleStatement> body)
		{
			this.modifiers = modifiers;
			this.type = type;
			this.name = name;
			this.required = required;
			this.optional = optional;
			this.body = body;
		}
	}
}