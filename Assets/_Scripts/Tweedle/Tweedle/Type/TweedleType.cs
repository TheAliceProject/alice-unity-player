namespace Alice.Tweedle
{
    abstract public class TweedleType
    {
		public string Name
		{
			get { return name; }
		}

		private readonly string name;
		private readonly TweedleType impliedType;

		public TweedleType(string name)
		{
			this.name = name;
			this.impliedType = null;
		}

		public TweedleType(string name, TweedleType impliedType)
		{
			this.name = name;
			this.impliedType = impliedType;
		}

		public bool AcceptsType(TweedleType type)
		{
			return this == type || (type.impliedType != null && AcceptsType(type.impliedType));
		}
	}
}