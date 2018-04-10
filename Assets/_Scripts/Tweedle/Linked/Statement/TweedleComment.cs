namespace Alice.Tweedle
{
	public class TweedleComment : TweedleStatement
	{
		private string text;

		public TweedleComment(string text)
		{
			this.text = text;
		}
	}
}