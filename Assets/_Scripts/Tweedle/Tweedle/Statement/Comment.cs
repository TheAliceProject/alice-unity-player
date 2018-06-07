namespace Alice.Tweedle
{
	public class Comment : TweedleStatement
	{
		private string text;

		public Comment(string text)
		{
			this.text = text;
		}

		public override void Execute(TweedleFrame frame)
		{
			frame.Next();
		}
	}
}