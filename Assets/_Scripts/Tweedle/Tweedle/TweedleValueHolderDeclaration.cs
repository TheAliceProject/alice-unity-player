using System;

namespace Alice.Tweedle
{
	public abstract class TweedleValueHolderDeclaration
	{
		public string Name
		{
			get { return name; }
		}

		public TweedleType Type
		{
			get { return type; }
		}

		private string name;
		private TweedleType type;

		public TweedleValueHolderDeclaration(TweedleType type, string name)
		{
			this.type = type;
			this.name = name;
		}
	}
}