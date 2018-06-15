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

		public override void Execute(TweedleFrame frame, Action next)
		{
			next();
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return ExecutionStep.NOOP;
		}
	}
}