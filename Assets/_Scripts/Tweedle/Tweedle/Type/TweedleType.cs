namespace Alice.Tweedle
{
    abstract public class TweedleType
    {
		public string Name
		{
			get { return name; }
            set { name = value; }
		}

		private string name;

		public TweedleType(string name)
		{
			this.name = name;
		}
    }
}