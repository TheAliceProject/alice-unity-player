using System;

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

		internal void InitializeValue(TweedleFrame frame)
		{
			if (Initializer == null)
			{
				throw new TweedleRuntimeException("Absent Initializer. Unable to initialize variable <" + Name + ">.");
			}
			Initializer.Evaluate(frame.ExecutionFrame(val =>
			{
				frame.Next(val);
			}));
		}
	}
}