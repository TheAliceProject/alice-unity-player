using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
	// public class TweedleEnum : TweedleTypeDeclaration
	// {
	// 	List<TweedleEnumValue> values = new List<TweedleEnumValue>();

	// 	public List<string> Values
	// 	{
	// 		get { return values.Select(v => v.Name).ToList(); }
	// 	}

	// 	public TweedleEnum(string name,
	// 		List<TField> properties,
	// 		List<TMethod> methods,
	// 		List<TweedleConstructor> constructors)
	// 		: base(name, properties, methods, constructors)
	// 	{
	// 	}

	// 	public void AddEnumValue(TweedleEnumValue value)
	// 	{
	// 		values.Add(value);
	// 	}

	// 	internal override TweedleEnum AsEnum(ExecutionScope scope)
	// 	{
	// 		return this;
	// 	}
	// }

	// // Implement as TweedleObject with only readonly fields
	// public class TweedleEnumValue
	// {
	// 	public string Name { get; }
	// 	public TweedleEnum EnumType { get; }
	// 	NamedArgument[] arguments;

	// 	public TweedleEnumValue(TweedleEnum type, string name, NamedArgument[] arguments)
	// 	{
	// 		EnumType = type;
	// 		Name = name;
	// 		this.arguments = arguments;
	// 	}
	// }
}