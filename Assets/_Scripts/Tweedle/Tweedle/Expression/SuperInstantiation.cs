using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class SuperInstantiation : TweedleExpression
	{
		Dictionary<string, TweedleExpression> Arguments { get; }

		public SuperInstantiation(Dictionary<string, TweedleExpression> arguments)
			: base(null)
		{
			Arguments = arguments;
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			ConstructorFrame cFrame = (ConstructorFrame) frame;
			cFrame.SuperInstantiate(Arguments);
		}
	}
}