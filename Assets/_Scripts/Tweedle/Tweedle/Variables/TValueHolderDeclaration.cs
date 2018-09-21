using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	/// <summary>
	/// Abstract declaration of a value holder.
	/// </summary>
	public abstract class TValueHolderDeclaration : IValueHolderDeclaration
	{
		public string Name { get; }
		public TTypeRef Type { get; }
		public ITweedleExpression Initializer { get; }

		public TValueHolderDeclaration(TTypeRef inType, string inName)
		{
			Type = inType;
			Name = inName;
			Initializer = null;
		}

		public TValueHolderDeclaration(TTypeRef inType, string inName, ITweedleExpression inInitializer)
		{
			Type = inType;
			Name = inName;
			Initializer = inInitializer;
		}

		internal ExecutionStep AsInitializerStep(ExecutionScope inScope)
		{
			if (Initializer == null)
			{
				throw new TweedleRuntimeException("Absent Initializer. Unable to initialize variable <" + Name + ">.");
			}
			return Initializer.AsStep(inScope);
		}

		internal bool Accepts(TValue inValue, ExecutionScope inScope)
		{
            return inValue != TValue.UNDEFINED && inValue.Type.CanCast(Type.Get(inScope));
		}

		public string ToTweedle()
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