namespace Alice.Tweedle
{
	public class TweedleTypes
	{
		public static readonly TweedlePrimitiveType NUMBER = new TweedlePrimitiveType("Number", null);
		public static readonly TweedleDecimalNumberType DECIMAL_NUMBER = new TweedleDecimalNumberType(NUMBER);
		public static readonly TweedleWholeNumberType WHOLE_NUMBER = new TweedleWholeNumberType(NUMBER);
		public static readonly TweedleBooleanType BOOLEAN = new TweedleBooleanType();
		public static readonly TweedleTextStringType TEXT_STRING = new TweedleTextStringType();

        //TODO Move these off the types class?
		public static readonly TweedlePrimitiveValue<bool> TRUE = BOOLEAN.Instantiate(true);
		public static readonly TweedlePrimitiveValue<bool> FALSE = BOOLEAN.Instantiate(false);

        public static readonly TweedlePrimitiveType[] PRIMITIVE_TYPES =
			{NUMBER, DECIMAL_NUMBER, WHOLE_NUMBER, BOOLEAN, TEXT_STRING};
	}
}