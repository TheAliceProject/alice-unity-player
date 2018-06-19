using System;
using Alice.VM;

namespace Alice.Tweedle
{
	public class Comment : TweedleStatement
	{
		string text;

		public Comment(string text)
		{
			this.text = text;
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return ExecutionStep.NOOP;
		}
	}
}