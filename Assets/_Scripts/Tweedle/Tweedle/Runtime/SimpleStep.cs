using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class SimpleStep : EvaluationStep
	{
		Func<TweedleValue> body;

		public SimpleStep(Func<TweedleValue> body)
		{
			this.body = body;
		}

		internal override bool Execute()
		{
			result = body.Invoke();
			return MarkCompleted();
		}
	}
}