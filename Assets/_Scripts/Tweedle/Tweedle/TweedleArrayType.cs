namespace Alice.Tweedle
{
	public class TweedleArrayType : TweedleType
	{
		private TweedleType type;

		public TweedleArrayType(TweedleType type) 
			: base(type.Name + "[]")
		{
		}

    }
}