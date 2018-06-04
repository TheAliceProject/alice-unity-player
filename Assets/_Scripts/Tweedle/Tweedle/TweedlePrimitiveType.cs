namespace Alice.Tweedle
{
	public class TweedlePrimitiveType : TweedleType
	{
		internal TweedlePrimitiveType(string name, TweedleType impliedType)
			: base(name, impliedType)
		{
		}
	}

	public abstract class TweedlePrimitiveType<V> : TweedlePrimitiveType
	{
		public TweedlePrimitiveType(string name, TweedleType impliedType)
			: base(name, impliedType)
		{
		}

		public TweedlePrimitiveValue<V> Instantiate(V value)
		{
			return new TweedlePrimitiveValue<V>(value, this);
		}

		internal override string ValueToString(TweedleValue value)
		{
			return ((TweedlePrimitiveValue<V>)value).Value.ToString();
		}
	}

	// TODO Add explicit Number type?
	// public class TweedleNumberType : TweedlePrimitiveType { }


	public class TweedleDecimalNumberType : TweedlePrimitiveType<double>
	{
		public TweedleDecimalNumberType(TweedleType parent)
			: base("DecimalNumber", parent)
		{
		}

		internal override double ValueToDouble(TweedleValue value)
		{
			return ((TweedlePrimitiveValue<double>)value).Value;
		}
	}

	public class TweedleWholeNumberType : TweedlePrimitiveType<int>
	{
		public TweedleWholeNumberType(TweedleType parent)
			: base("WholeNumber", parent)
		{
		}

		internal override double ValueToDouble(TweedleValue value)
		{
			return ((TweedlePrimitiveValue<int>)value).Value;
		}
	}

	public class TweedleTextStringType : TweedlePrimitiveType<string>
	{
		public TweedleTextStringType()
			: base("TextString", null)
		{
		}
	}

	public class TweedleBooleanType : TweedlePrimitiveType<bool>
	{
		public TweedleBooleanType()
			: base("Boolean", null)
		{
		}
	}

}