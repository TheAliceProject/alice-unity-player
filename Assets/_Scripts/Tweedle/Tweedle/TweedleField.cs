using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleField : TweedleValueHolderDeclaration
    {
        private List<string> modifiers;
        private TweedleExpression initializer;

		public List<string> Modifiers
		{
			get { return modifiers; }
		}


        public TweedleField(List<string> modifiers, TweedleType type, string name)
            : base(type, name)
		{
            this.modifiers = modifiers;
        }


        public TweedleField(List<string> modifiers, TweedleType type, string name, TweedleExpression initializer)
            : base(type, name)
        {
            this.modifiers = modifiers;
            this.initializer = initializer;
        }
	}
}