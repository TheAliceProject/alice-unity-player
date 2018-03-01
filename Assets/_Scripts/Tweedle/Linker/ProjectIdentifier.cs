namespace Alice.Linker
{
	[System.Serializable]
    public class ProjectIdentifier
    {
		public ProjectType Type
		{
			get
			{
				return (ProjectType)System.Enum.Parse(typeof(ProjectType), type);
			}
		}

		public string aliceVersion;
		public string type;
		public string id;
		public string version;
		public string formatVersion;
	}
}