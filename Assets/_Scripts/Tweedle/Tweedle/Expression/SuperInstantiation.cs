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

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			ConstructorFrame superFrame = ((ConstructorFrame)frame).SuperFrame(Arguments);
			return superFrame.InvokeStep();
		}
	}
}