namespace Alice.Tweedle
{
	public class TweedlePrimitiveType : TweedleType
	{
		internal TweedlePrimitiveType(string name, TweedleType impliedType) 
			: base(name, impliedType)
		{
		}
	}

	public class TweedlePrimitiveType<T> : TweedlePrimitiveType
	{
		public TweedlePrimitiveType(string name, TweedleType impliedType)
			: base(name, impliedType)
		{
		}

		public TweedlePrimitiveValue<T> Instantiate(T value)
		{
			return new TweedlePrimitiveValue<T>(value, this);
		}
	}
}