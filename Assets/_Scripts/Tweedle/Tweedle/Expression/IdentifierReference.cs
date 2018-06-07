namespace Alice.Tweedle
{
	public class IdentifierReference : TweedleExpression
	{
		private string name;

		public string Name
		{
			get
			{
				return name;
			}
		}

		public IdentifierReference(string name)
			: base(null)
		{
			this.name = name;
		}

		override public void Evaluate(TweedleFrame frame)
		{
			// TODO track on execution frame
			// return frame.getValueFor(this);
		}
	}
}