using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class TweedleValueHolderDeclaration
	{
		public TweedleType Type { get; }
		public string Name { get; }
		public TweedleExpression Initializer { get; }

		public TweedleValueHolderDeclaration(TweedleType type, string name)
		{
			Type = type;
			Name = name;
			Initializer = null;
		}

		public TweedleValueHolderDeclaration(TweedleType type, string name, TweedleExpression initializer)
		{
			Type = type;
			Name = name;
			Initializer = initializer;
		}

		internal NotifyingEvaluationStep AsInitializerStep(TweedleFrame frame)
		{
			if (Initializer == null)
			{
				throw new TweedleRuntimeException("Absent Initializer. Unable to initialize variable <" + Name + ">.");
			}
			return Initializer.AsStep(frame);
		}

		internal bool Accepts(TweedleValue value, TweedleFrame frame)
		{
			return value != null && Type.AsDeclaredType(frame).AcceptsType(value.Type);
		}

		internal string ToTweedle()
		{
			if (Initializer == null)
			{
				return Type.Name + " " + Name;
			}
			else
			{
				return Type.Name + " " + Name + " <- " + Initializer;
			}
		}
	}
}