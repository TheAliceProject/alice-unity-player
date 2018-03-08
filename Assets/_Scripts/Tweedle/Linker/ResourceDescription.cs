using System.Collections.Generic;

namespace Alice.Linker
{
	[System.Serializable]
	public class ResourceDescription
	{
		public Resource.ContentType ContentType
		{
			get
			{
				string type = System.Text.RegularExpressions.Regex.Replace(contentType, @"([.,-][a-z]|\b[a-z])", m => m.Value.ToUpper());
				type = type.Replace("-", string.Empty);
				try
				{
					return (Resource.ContentType)System.Enum.Parse(typeof(Resource.ContentType), type);
				}
				catch (System.Exception)
				{
					return Resource.ContentType.NULL;
				}
			}
		}

		public Resource.FormatType FormatType
		{
			get
			{
				string type = System.Text.RegularExpressions.Regex.Replace(format, @"([.,-][a-z]|\b[a-z])", m => m.Value.ToUpper());
				type = type.Replace("-", string.Empty);
				try
				{
					return (Resource.FormatType)System.Enum.Parse(typeof(Resource.FormatType), type);
				}
				catch (System.Exception)
				{
					return Resource.FormatType.NULL;
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