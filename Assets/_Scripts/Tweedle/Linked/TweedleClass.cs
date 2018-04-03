using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleClass : TweedleType
    {
        public TweedleClass Super
        {
            get { return super; }
            set { super = value; }
        }

        private TweedleClass super;
        private List<TweedleProperty> properties;
        private List<TweedleMethod> methods;
		public List<TweedleConstructor> constructors { get; internal set; }

		public TweedleClass(string name) : base(name)
        {
        }

		public TweedleClass(string name, TweedleClass super) : base(name)
		{
			this.super = super;
		}

		public TweedleObject instantiate(VM.TweedleFrame frame, TweedleValue[] args)
        {
            return null;
        }
    }
}