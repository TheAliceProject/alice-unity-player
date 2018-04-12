using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleMethod
    {
		public string Name
		{
			get { return name; }
		}

		private List<string> modifiers;
		private TweedleType type;
        private string name;
        private List<TweedleRequiredParameter> required;
        private List<TweedleOptionalParameter> optional;
		private List<TweedleStatement> body;

        public TweedleMethod(TweedleType type, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> body)
		{
			this.type = type;
			this.name = name;
			this.required = required;
			this.optional = optional;
			this.body = body;
		}

        public TweedleMethod(List<string> modifiers, TweedleType type, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> body)
            : this(type, name, required, optional, body)
		{
			this.modifiers = modifiers;
		}
	}
}