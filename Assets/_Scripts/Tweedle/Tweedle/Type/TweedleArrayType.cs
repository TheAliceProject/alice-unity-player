namespace Alice.Tweedle
{
	public class TweedleArrayType : TweedleType
	{
		private TweedleType valueType;

		public TweedleType ValueType
		{
			get { return valueType; }
		}

		public TweedleArrayType()
			: base("[]")
		{
			this.valueType = null;
		}

		public TweedleArrayType(TweedleType valueType)
			: base(valueType?.Name + "[]")
		{
			this.valueType = valueType;
		}

		public override bool AcceptsType(TweedleType type)
		{
			return this == type ||
							((type is TweedleArrayType) &&
							(valueType == null || valueType.AcceptsType(((TweedleArrayType)type).valueType)));
		}
	}
}