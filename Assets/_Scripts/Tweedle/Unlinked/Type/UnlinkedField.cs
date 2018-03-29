namespace Alice.Tweedle.Unlinked
{
	public class UnlinkedField : UnlinkedClassBodyDeclaration
	{
		private UnlinkedStatement initializer;

		public UnlinkedField(string name, UnlinkedStatement initializer) : base(name)
		{
			this.initializer = initializer;
		}
	}
}