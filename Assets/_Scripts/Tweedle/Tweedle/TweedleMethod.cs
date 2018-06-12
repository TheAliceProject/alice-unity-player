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

		internal void Invoke(TweedleFrame frame, Dictionary<string, TweedleExpression> arguments)
		{
			EvaluateArguments(frame, arguments);

			Dictionary<string, TweedleValue> argValues = new Dictionary<string, TweedleValue>();
			foreach (KeyValuePair<string, TweedleExpression> pair in arguments)
			{
				pair.Value.Evaluate(frame, value => argValues.Add(pair.Key, value));
			}
			Body.ExecuteInSequence(frame, () => { });
		}

		private void EvaluateArguments(TweedleFrame frame, Dictionary<string, TweedleExpression> arguments)
		{
			foreach (TweedleRequiredParameter req in RequiredParameters)
			{
				TweedleExpression argExp;
				if (arguments.TryGetValue(req.Name, out argExp))
				{
					argExp.Evaluate(frame, value => frame.SetLocalValue(req, value));
				}
				else
				{
					throw new TweedleLinkException("Invalid method call on " + Name + ". Missing value for required parameter " + req.Name);
				}
			}
			foreach (TweedleOptionalParameter opt in OptionalParameters)
			{
				TweedleExpression argExp;
				if (arguments.TryGetValue(opt.Name, out argExp))
				{
					argExp.Evaluate(frame, value => frame.SetLocalValue(opt, value));
				}
				else
				{
					opt.Initializer.Evaluate(frame, value => frame.SetLocalValue(opt, value));
				}
			}
		}

		public bool IsStatic()
		{
			return Modifiers.Contains("static");
		}
	}
}