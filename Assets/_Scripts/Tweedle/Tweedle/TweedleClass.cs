using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleClass : TweedleTypeDeclaration, InvocableMethodHolder
	{
		InvocableMethodHolder superClass;

		public string SuperClassName
		{
			get { return superClass?.Name; }
		}

		public TweedleClass(string name,
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
			: base(name, properties, methods, constructors)
		{
			superClass = null;
		}

		public TweedleClass(string name, TweedleTypeReference super,
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
			: base(name, super, properties, methods, constructors)
		{
			superClass = super;
		}

		public TweedleClass(string name, string super,
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
			: this(name, new TweedleTypeReference(super), properties, methods, constructors)
		{
		}

		public TweedleObject Instantiate(TweedleFrame frame, TweedleValue[] args)
		{
			return new TweedleObject(this);
		}


		public override TweedleMethod MethodNamed(string methodName)
		{
			TweedleMethod method = base.MethodNamed(methodName);
			if (method != null)
			{
				return method;
			}
			if (superClass != null)
			{
				return superClass.MethodNamed(methodName);
			}
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