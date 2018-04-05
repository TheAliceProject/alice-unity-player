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

        public List<TweedleProperty> properties;
        public List<TweedleMethod> methods;
		public List<TweedleConstructor> constructors { get; internal set; }

		private TweedleClass super;

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