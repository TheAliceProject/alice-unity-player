using System;
using System.Collections.Generic;
using Alice.VM;

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

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			throw new NotImplementedException();
		}
	}
}