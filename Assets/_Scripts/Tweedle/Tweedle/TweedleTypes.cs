namespace Alice.Tweedle
{
	public class TweedleTypes
	{
		public static readonly TweedlePrimitiveType NUMBER = new TweedlePrimitiveType("Number", null);
		public static readonly TweedlePrimitiveType<double> DECIMAL_NUMBER = new TweedlePrimitiveType<double>("DecimalNumber", NUMBER);
		public static readonly TweedlePrimitiveType<int> WHOLE_NUMBER = new TweedlePrimitiveType<int>("WholeNumber", NUMBER);
		public static readonly TweedlePrimitiveType<bool> BOOLEAN = new TweedlePrimitiveType<bool>("Boolean", null);
		public static readonly TweedlePrimitiveType<string> TEXT_STRING = new TweedlePrimitiveType<string>("TextString", null);

		public static readonly TweedlePrimitiveType[] PRIMITIVE_TYPES =
			{NUMBER, DECIMAL_NUMBER, WHOLE_NUMBER, BOOLEAN, TEXT_STRING};
	}
}