namespace Alice.Tweedle
{
	class ValueHolder
	{
		TweedleValue tweedleValue;

		public TweedleType Type { get; }
		public TweedleValue Value
		{
			get
			{
				return tweedleValue;
			}

			set
			{
				if (Type.AcceptsType(value.Type))
				{
					tweedleValue = value;
				}
				else
				{
					throw new TweedleRuntimeException("Unable to treat " + value + " as type " + Type);
				}
			}
		}

		public ValueHolder(TweedleType type, TweedleValue tweedleValue)
		{
			Type = type;
			Value = tweedleValue;
		}
	}
}