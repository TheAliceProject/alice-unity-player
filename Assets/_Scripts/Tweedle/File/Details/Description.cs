using System.Collections.Generic;

namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class Description
	{
		public string name;
		public string icon;
		public List<string> tags;
		public List<string> groupTags;
		public List<string> themeTags;
	}
}