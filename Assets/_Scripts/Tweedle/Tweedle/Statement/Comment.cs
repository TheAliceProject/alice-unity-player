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

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			return null;
		}
	}
}