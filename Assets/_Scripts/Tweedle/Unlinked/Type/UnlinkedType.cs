namespace Alice.Tweedle.Unlinked
{
	abstract public class UnlinkedType
	{
		public string Name
		{
			get { return name; }
		}

		private string name;

		public UnlinkedType(string name)
		{
			this.name = name;
		}
	}
}