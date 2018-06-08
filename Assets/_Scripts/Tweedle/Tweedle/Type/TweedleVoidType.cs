namespace Alice.Tweedle
{
	public class TweedleVoidType : TweedleType
	{
		public static TweedleVoidType VOID = new TweedleVoidType();

		private TweedleVoidType()
			: base("void", null)
		{
		}
	}
}