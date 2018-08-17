using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleClass : TweedleTypeDeclaration
	{
		TweedleType superClass;

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

		internal override TweedleClass AsClass(ExecutionScope scope)
		{
			return this;
		}

		internal TweedleClass SuperClass(ExecutionScope scope)
		{
			if (SuperClassName != null)
			{
				return scope.ClassNamed(SuperClassName);
			}
			return null;
		}

		internal TweedleConstructor ConstructorWithArgs(Dictionary<string, TweedleExpression> arguments)
		{
			return Constructors.FindLast(cstr => cstr.ExpectsArgs(arguments));
		}

		internal override TweedleField Field(ExecutionScope scope, string fieldName)
		{
			TweedleField field = base.Field(scope, fieldName);
			if (field != null)
			{
				return field;
			}
			if (superClass != null)
			{
				return SuperClass(scope).Field(scope, fieldName);
			}
			return null;
		}


		public override TweedleMethod MethodNamed(ExecutionScope scope, string methodName)
		{
			TweedleMethod method = base.MethodNamed(scope, methodName);
			if (method != null)
			{
				return method;
			}
			if (superClass != null)
			{
				return SuperClass(scope).MethodNamed(scope, methodName);
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

		// NB There is no guaranteed order for the execution of steps.
		//    Make sure they do not conflict or change this to enforce order, most likely by class hierarchy.
		internal void AddInitializationSteps(List<ExecutionStep> steps, ExecutionScope scope, TweedleObject tweedleObject)
		{
			if (superClass != null)
			{
				SuperClass(scope).AddInitializationSteps(steps, scope, tweedleObject);
			}
			foreach (TweedleField field in Properties)
			{
				if (field.Initializer != null)
				{
					steps.Add(field.InitializeField(scope, tweedleObject));
				}
			}
		}
	}
}