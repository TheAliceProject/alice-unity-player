namespace Alice.Tweedle.Unlinked
{
	public class UnlinkedClass : UnlinkedType
	{
		public string SuperclassName
		{
			get { return superclassName; }
		}

		private string superclassName;

		public UnlinkedClass(string className)
			: base(className)
		{
			superclassName = null;
		}

		public UnlinkedClass(string className, string superclassName)
			: base(className)
		{
			this.superclassName = superclassName;
		}
	}
}