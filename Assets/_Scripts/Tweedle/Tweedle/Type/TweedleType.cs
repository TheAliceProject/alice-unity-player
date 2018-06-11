namespace Alice.Tweedle
{
	abstract public class TweedleType
	{
		public string Name { get; }
		public TweedleType ImpliedType { get; }

		public TweedleType(string name)
		{
			Name = name;
			ImpliedType = null;
		}

		public TweedleType(string name, TweedleType impliedType)
		{
			Name = name;
			ImpliedType = impliedType;
		}

		internal virtual double ValueToDouble(TweedleValue value)
		{
			throw new TweedleRuntimeException("This type (" + this + ") cannot convert the value " + value + "to a double.");
		}

		internal virtual TweedleType AsDeclaredType(TweedleFrame frame)
		{
			return this;
		}

		internal virtual TweedleClass AsClass(TweedleFrame frame)
		{
			throw new TweedleRuntimeException("Attempt to treat type " + Name + " as a class can not be processed.");
		}

		internal virtual TweedleEnum AsEnum(TweedleFrame frame)
		{
			throw new TweedleRuntimeException("Attempt to treat type " + Name + " as an enum can not be processed.");
		}

		internal virtual int ValueToInt(TweedleValue value)
		{
			throw new TweedleRuntimeException("This type (" + Name + ") cannot convert the value " + value + "to an int.");
		}

		internal virtual string ValueToString(TweedleValue value)
		{
			return "a" + Name;
		}

		internal virtual bool ValueToBoolean(TweedleValue value)
		{
			throw new TweedleRuntimeException("This type (" + Name + ") cannot convert the value " + value + "to a bool.");
		}

		virtual public bool AcceptsType(TweedleType type)
		{
			return this == type || (type.ImpliedType != null && AcceptsType(type.ImpliedType));
		}
	}
}