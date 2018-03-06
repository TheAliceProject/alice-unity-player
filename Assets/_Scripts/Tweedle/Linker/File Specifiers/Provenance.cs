namespace Alice.Linker
{
	[System.Serializable]
	public class Provenance
	{
		public string aliceVersion;
		public string creationYear;
		public string creator;

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n" + aliceVersion;
			str += "\n" + creationYear;
			str += "\n" + creator;
			return str;
		}
	}
}