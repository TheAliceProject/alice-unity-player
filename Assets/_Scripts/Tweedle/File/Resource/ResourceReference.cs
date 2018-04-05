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
				string type_regex = System.Text.RegularExpressions.Regex.Replace(type, @"([.,-][a-z]|\b[a-z])", m => m.Value.ToUpper());
				type_regex = type_regex.Replace("-", string.Empty);
				try
				{
					return (ContentType)System.Enum.Parse(typeof(ContentType), type_regex);
				}
				catch (System.Exception)
				{
					return ContentType.NULL;
				}
			}
		}

		public string FormatType
		{
			get { return format; }
		}


		public string id;
		public string type;
		public string format;
		public List<string> files;

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n" + id;
			str += "\n" + type;
			str += "\n" + format;
			for (int i = 0; i < files.Count; i++)
				str += "\n" + files[i];
			return str;
		}
	}
}