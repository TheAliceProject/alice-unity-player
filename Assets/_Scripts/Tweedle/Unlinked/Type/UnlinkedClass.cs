using System.Collections.Generic;

namespace Alice.Tweedle.Unlinked
{
	public class UnlinkedClass : UnlinkedType
	{
		public string SuperclassName
		{
			get { return superclassName; }
		}

		public List<UnlinkedField> fields { get; internal set; }
		public List<UnlinkedMethod> methods { get; internal set; }
		public List<UnlinkedConstructor> constructors { get; internal set; }

		private string superclassName;

		public UnlinkedClass(string className)
			: base(className)
		{
			superclassName = null;
		}

		public UnlinkedClass(string className, string superclassName)
			: base(className)
		{
			this.superclassName = superclassName;
		}
	}
}