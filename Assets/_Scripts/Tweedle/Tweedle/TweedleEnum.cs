using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
    public class TweedleEnum : TweedleTypeDeclaration
    {
		private List<TweedleEnumValue> values = new List<TweedleEnumValue>();

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

		public TweedleObject Instantiate(TweedleFrame frame, Dictionary<string, TweedleValue> arguements)
        {
            return null;
        }

		public override void Invoke(TweedleFrame frame, TweedleObject target, string methodName, Dictionary<string, TweedleValue> arguments)
		{
			throw new System.NotImplementedException();
		}
	}

	public class TweedleEnumValue : TweedleValue
	{
		private string name;
		private Dictionary<string, TweedleExpression> arguments;

		public string Name
		{
			get { return name; }
		}

		public TweedleEnumValue(TweedleEnum type, string name, Dictionary<string, TweedleExpression> arguments)
			:base(type)
		{
			this.name = name;
			this.arguments = arguments;
		}
	}
}