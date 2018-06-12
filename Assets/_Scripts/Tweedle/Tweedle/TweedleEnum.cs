using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
	public class TweedleEnum : TweedleTypeDeclaration
	{
		List<TweedleEnumValue> values = new List<TweedleEnumValue>();

		public List<string> Values
		{
			get { return values.Select(v => v.Name).ToList(); }
		}

		public TweedleEnum(string name,
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors)
			: base(name, properties, methods, constructors)
		{
		}

		public void AddEnumValue(TweedleEnumValue value)
		{
			values.Add(value);
		}

		internal override TweedleEnum AsEnum(TweedleFrame frame)
		{
			return this;
		}
	}

	public class TweedleEnumValue : TweedleValue
	{
		public string Name { get; }
		public TweedleEnum EnumType { get; }
		Dictionary<string, TweedleExpression> arguments;

		public TweedleEnumValue(TweedleEnum type, string name, Dictionary<string, TweedleExpression> arguments)
			: base(type)
		{
			EnumType = type;
			Name = name;
			this.arguments = arguments;
		}

		internal override TweedleMethod MethodNamed(TweedleFrame frame, string methodName)
		{
			return EnumType.MethodNamed(frame, methodName);
		}
	}
}