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

		public string id;
		public string version;
		public string type;

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n" + id;
			str += "\n" + version;
			str += "\n" + type;
			return str;
		}
	}
}