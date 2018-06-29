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

		internal override NotifyingEvaluationStep AsStep(NotifyingStep next, TweedleFrame frame)
		{
			ConstructorFrame superFrame = ((ConstructorFrame)frame).SuperFrame(Arguments);
			return superFrame.InvocationStep(frame.StackWith("super()"), Arguments, next);
		}
	}
}