using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
    public class TweedleEnum : TweedleTypeDeclaration
    {
		private List<TweedleEnumValue> values;

		public List<string> Values
		{
			get { return values.Select(v => v.Name).ToList(); }
		}

        public TweedleEnum(string name, 
			List<TweedleField> properties,
			List<TweedleMethod> methods,
			List<TweedleConstructor> constructors,
			List<TweedleEnumValue> values)
			: base(name, properties, methods, constructors)
        {
			this.values = values;
        }

		public TweedleObject Instantiate(TweedleFrame frame, TweedleValue[] args)
        {
            return null;
        }

		public override void Invoke(TweedleFrame frame, TweedleObject target, string methodName, Dictionary<string, TweedleValue> arguments)
		{
			throw new System.NotImplementedException();
		}
	}

	public class TweedleEnumValue
	{
		private string name;
		private Dictionary<string, TweedleExpression> arguments;

		public string Name
		{
			get { return name; }
		}

		public TweedleEnumValue(string name, Dictionary<string, TweedleExpression> arguments)
		{
			this.name = name;
			this.arguments = arguments;
		}
	}
}