namespace Alice.Tweedle.Unlinked
{
	abstract public class UnlinkedClassBodyDeclaration
	{
		public string Name
		{
			get { return name; }
		}

		private string name;

		public UnlinkedClassBodyDeclaration(string name)
		{
			this.name = name;
		}
	}
}