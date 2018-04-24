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

        public TweedleType Type
        {
            get
            {
                return type;
            }
        }

        private string name;
        private List<TweedleRequiredParameter> required;

        public List<TweedleRequiredParameter> RequiredParameters
        {
            get
            {
                return required;
            }
        }

        private List<TweedleOptionalParameter> optional;
        public List<TweedleOptionalParameter> OptionalParameters
        {
            get
            {
                return optional;
            }
        }
		private List<TweedleStatement> body;

        public List<TweedleStatement> Body
        {
            get
            {
                return body;
            }
        }

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