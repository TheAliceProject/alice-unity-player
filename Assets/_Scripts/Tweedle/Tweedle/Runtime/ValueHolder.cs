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
				CheckType(Type, value);
				tweedleValue = value;
			}
		}

		public ValueHolder(TweedleType type, TweedleValue tweedleValue)
		{
			CheckType(type, tweedleValue);
			Type = type;
			this.tweedleValue = tweedleValue;
		}

		void CheckType(TweedleType type, TweedleValue value)
		{
			if (value == null || !type.AcceptsType(value.Type))
			{
				throw new TweedleRuntimeException("Unable to treat " + value + " as type " + type);
			}
		}
	}
}