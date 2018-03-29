namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class MetaData
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
