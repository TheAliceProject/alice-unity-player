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

		internal EvaluationStep InitializerStep(TweedleFrame frame)
		{
			if (Initializer == null)
			{
				return TweedleNull.NULL.AsStep(frame);
				// OR
				// throw new TweedleRuntimeException("Absent Initializer. Unable to initialize variable <" + Name + ">.");
			}
			else
			{
				return Initializer.AsStep(frame);
			}
		}

		internal bool Accepts(TweedleValue value)
		{
			return value != null && Type.AcceptsType(value.Type);
		}
	}
}