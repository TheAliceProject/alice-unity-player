using System.Collections.Generic;

namespace Alice.Linker
{
	[System.Serializable]
	public class Preference
	{
		public string name;
		public List<Deprecated.StatementDescription> value; // TODO
	}
}