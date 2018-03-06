using System.Collections.Generic;

namespace Alice.Linker
{
	[System.Serializable]
	public class Description
	{
		public string name;
		public string icon;
		public List<string> tags;
		public List<string> groupTags;
		public List<string> themeTags;

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n" + name;
			str += "\n" + icon;
			for (int i = 0; i < tags.Count; i++)
				str += "\n" + tags.ToString();
			for (int i = 0; i < groupTags.Count; i++)
				str += "\n" + groupTags.ToString();
			for (int i = 0; i < themeTags.Count; i++)
				str += "\n" + themeTags.ToString();
			return str;
		}
	}
}