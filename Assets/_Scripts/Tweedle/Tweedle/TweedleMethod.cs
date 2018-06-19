using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleMethod
	{
		public List<string> Modifiers { get; }
		public string Name { get; }
		public TweedleType Type { get; }
		public List<TweedleRequiredParameter> RequiredParameters { get; }
		public List<TweedleOptionalParameter> OptionalParameters { get; }
		public BlockStatement Body { get; }

		public TweedleMethod(TweedleType resultType, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> statements)
		{
			Type = resultType;
			Name = name;
			RequiredParameters = required;
			OptionalParameters = optional;
			Body = new BlockStatement(statements);
		}

		public TweedleMethod(List<string> modifiers, TweedleType type, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> statements)
			: this(type, name, required, optional, statements)
		{
			Modifiers = modifiers;
		}

		// TODO use args for method identification not just call validation
		public bool ExpectsArgs(Dictionary<string, TweedleExpression> arguments)
		{
			foreach (TweedleRequiredParameter param in RequiredParameters)
			{
				if (!arguments.ContainsKey(param.Name))
				{
					return false;
				}
			}
			return true;
		}

		public virtual bool IsStatic()
		{
			return Modifiers.Contains("static");
		}
	}
}