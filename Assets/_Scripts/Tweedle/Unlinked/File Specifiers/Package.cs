namespace Alice.Tweedle.Unlinked
{
	[System.Serializable]
	public class Package
	{
		public string formatVersion;
		public ProjectIdentifier identifier;

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n" + formatVersion;
			str += "\n" + identifier.ToString();
			return str;
		}
	}
}
