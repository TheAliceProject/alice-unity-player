namespace Alice.Tweedle
{
	public class TweedleNull : TweedleValue
	{
		public static TweedleNull NULL = new TweedleNull();

		private TweedleNull()
			: base(TweedleVoidType.VOID)
		{
		}
	}
}