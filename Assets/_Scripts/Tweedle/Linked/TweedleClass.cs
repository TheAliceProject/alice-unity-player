using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleClass : TweedleType
    {
        public TweedleType Super
        {
            get { return super; }
            set { super = value; }
        }

        public List<TweedleField> properties;
        public List<TweedleMethod> methods;
		public List<TweedleConstructor> constructors { get; internal set; }

		private TweedleType super;

		public TweedleClass(string name) : base(name)
        {
			this.properties = new List<TweedleField>();
			this.methods = new List<TweedleMethod>();
			this.constructors = new List<TweedleConstructor>();
		}

		public TweedleClass(string name, TweedleType super) : base(name)
		{
			this.super = super;
			this.properties = new List<TweedleField>();
			this.methods = new List<TweedleMethod>();
			this.constructors = new List<TweedleConstructor>();
		}

		public TweedleObject Instantiate(VM.TweedleFrame frame, TweedleValue[] args)
        {
            return null;
        }

		public override string ToString()
		{
			string str = Name + "\n";
			//for (int i = 0; i < properties.Count; i++)
			//	str += properties[i].Name + " ";
			//str += "\n";
			//for (int i = 0; i < methods.Count; i++)
			//	str += methods[i].Name + " ";
			//str += "\n";
			for (int i = 0; i < constructors.Count; i++)
				str += constructors[i].Name + " ";
			return str;
		}
	}
}