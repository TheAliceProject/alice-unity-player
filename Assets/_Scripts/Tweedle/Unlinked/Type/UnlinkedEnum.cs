using System.Collections.Generic;

namespace Alice.Tweedle.Unlinked
{
	public class UnlinkedEnum : UnlinkedType
	{
		public List<string> Values
		{
			get { return values; }
		}

		private List<string> values;

		public UnlinkedEnum(string name, List<string> values)
			: base(name)
		{
			this.values = values;
		}
	}
}