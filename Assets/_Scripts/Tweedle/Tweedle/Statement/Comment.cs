using Alice.VM;

namespace Alice.Tweedle
{
	public class Comment : TweedleStatement
	{
		string text;

		public Comment(string text)
		{
			this.text = text;
			Disable();
		}

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return null;
		}
	}
}