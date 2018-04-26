using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleClass : TweedleType, InvocableMethodHolder
    {
        public List<TweedleField> properties;
        public List<TweedleMethod> methods;
		public List<TweedleConstructor> constructors { get; internal set; }

		private InvocableMethodHolder superClass;

        public string SuperClassName
        {
            get { return superClass?.Name; }
       } 

		public TweedleClass(string name) 
			: base(name)
        {
			this.properties = new List<TweedleField>();
			this.methods = new List<TweedleMethod>();
			this.constructors = new List<TweedleConstructor>();
		}

		public TweedleClass(string name, TweedleTypeReference superClass) 
			: base(name, superClass)
		{
			this.superClass = superClass;
			this.properties = new List<TweedleField>();
			this.methods = new List<TweedleMethod>();
			this.constructors = new List<TweedleConstructor>();
		}

		public TweedleClass(string name, string super)
			: this(name, new TweedleTypeReference(super))
		{
		}

		public void Invoke(TweedleFrame frame, TweedleObject target, TweedleMethod method, TweedleValue[] arguments)
		{
			if (methods.Contains(method))
			{
				method.Invoke(frame, target, arguments);
			} else
			{
				superClass.Invoke(frame, target, method, arguments);
			}
		}

		public TweedleObject Instantiate(TweedleFrame frame, TweedleValue[] args)
        {
            return new TweedleObject(this);
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