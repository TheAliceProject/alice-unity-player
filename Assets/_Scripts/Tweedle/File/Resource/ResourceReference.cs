using System.Collections.Generic;

namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class ResourceReference
	{
		public virtual ContentType ContentType
		{
			get
			{
				string type = System.Text.RegularExpressions.Regex.Replace(contentType, @"([.,-][a-z]|\b[a-z])", m => m.Value.ToUpper());
				type = type.Replace("-", string.Empty);
				try
				{
					return (ContentType)System.Enum.Parse(typeof(ContentType), type);
				}
				catch (System.Exception)
				{
					return ContentType.NULL;
				}
			}
		}

		public FormatType FormatType
		{
			get
			{
				string type = System.Text.RegularExpressions.Regex.Replace(format, @"([.,-][a-z]|\b[a-z])", m => m.Value.ToUpper());
				type = type.Replace("-", string.Empty);
				try
				{
					return (FormatType)System.Enum.Parse(typeof(FormatType), type);
				}
				catch (System.Exception)
				{
					return FormatType.NULL;
				}
			}
		}

		public string id;
		public string contentType;
		public string format;
		public List<string> files;

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n" + id;
			str += "\n" + contentType;
			str += "\n" + format;
			for (int i = 0; i < files.Count; i++)
				str += "\n" + files[i];
			return str;
		}
	}
}