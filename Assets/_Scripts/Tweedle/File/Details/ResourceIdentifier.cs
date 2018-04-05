namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class ResourceIdentifier
	{
		public string id;
		public ContentType contentType;
		public string format;

		public ResourceIdentifier(string id, ContentType contentType, string format)
		{
			this.id = id;
			this.contentType = contentType;
			this.format = format;
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n" + id;
			str += "\n" + contentType;
			str += "\n" + format;
			return str;
		}
	}
}