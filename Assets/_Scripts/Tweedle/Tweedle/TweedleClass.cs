using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	// public class TClassType : TweedleTypeDeclaration
	// {
	// 	TType superClass;

	// 	public string SuperClassName
	// 	{
	// 		get { return superClass?.Name; }
	// 	}

	// 	public TClassType(string name,
	// 		List<TField> properties,
	// 		List<TMethod> methods,
	// 		List<TweedleConstructor> constructors)
	// 		: base(name, properties, methods, constructors)
	// 	{
	// 		superClass = null;
	// 	}

	// 	public TClassType(string name, TweedleTypeReference super,
	// 		List<TField> properties,
	// 		List<TMethod> methods,
	// 		List<TweedleConstructor> constructors)
	// 		: base(name, super, properties, methods, constructors)
	// 	{
	// 		superClass = super;
	// 	}

	// 	public TClassType(string name, string super,
	// 		List<TField> properties,
	// 		List<TMethod> methods,
	// 		List<TweedleConstructor> constructors)
	// 		: this(name, new TweedleTypeReference(super), properties, methods, constructors)
	// 	{
	// 	}

	// 	internal override TClassType AsClass(ExecutionScope scope)
	// 	{
	// 		return this;
	// 	}

	// 	internal TClassType SuperClass(ExecutionScope scope)
	// 	{
	// 		if (SuperClassName != null)
	// 		{
	// 			return scope.ClassNamed(SuperClassName);
	// 		}
	// 		return null;
	// 	}

	// 	internal TweedleConstructor ConstructorWithArgs(NamedArgument[] inArguments)
	// 	{
	// 		return Constructors.FindLast(cstr => cstr.ExpectsArgs(inArguments));
	// 	}

	// 	internal override TField Field(ExecutionScope scope, string fieldName)
	// 	{
	// 		TField field = base.Field(scope, fieldName);
	// 		if (field != null)
	// 		{
	// 			return field;
	// 		}
	// 		if (superClass != null)
	// 		{
	// 			return SuperClass(scope).Field(scope, fieldName);
	// 		}
	// 		return null;
	// 	}


	// 	public override TMethod MethodNamed(ExecutionScope scope, string methodName)
	// 	{
	// 		TMethod method = base.MethodNamed(scope, methodName);
	// 		if (method != null)
	// 		{
	// 			return method;
	// 		}
	// 		if (superClass != null)
	// 		{
	// 			return SuperClass(scope).MethodNamed(scope, methodName);
	// 		}
	// 		return null;
	// 	}

	// 	public override string ToString()
	// 	{
	// 		string str = Name + "\n";
	// 		//for (int i = 0; i < properties.Count; i++)
	// 		//	str += properties[i].Name + " ";
	// 		//str += "\n";
	// 		//for (int i = 0; i < methods.Count; i++)
	// 		//	str += methods[i].Name + " ";
	// 		//str += "\n";
	// 		for (int i = 0; i < constructors.Count; i++)
	// 			str += constructors[i].Name + " ";
	// 		return str;
	// 	}

	// 	// NB There is no guaranteed order for the execution of steps.
	// 	//    Make sure they do not conflict or change this to enforce order, most likely by class hierarchy.
	// 	internal void AddInitializationSteps(List<ExecutionStep> steps, ExecutionScope scope, TObject tweedleObject)
	// 	{
	// 		if (superClass != null)
	// 		{
	// 			SuperClass(scope).AddInitializationSteps(steps, scope, tweedleObject);
	// 		}
	// 		foreach (TField field in Properties)
	// 		{
	// 			if (field.Initializer != null)
	// 			{
	// 				steps.Add(field.InitializeField(scope, tweedleObject));
	// 			}
	// 		}
	// 	}
	// }
}