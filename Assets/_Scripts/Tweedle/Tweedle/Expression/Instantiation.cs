using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class Instantiation : TweedleExpression
	{
		internal Dictionary<string, TweedleExpression> Arguments { get; }

		public Instantiation(TweedleTypeReference type, Dictionary<string, TweedleExpression> arguments)
			: base(type)
		{
			Arguments = arguments;
			if (type == null)
			{
				UnityEngine.Debug.Log("Placeholder");
			}
		}

        internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			ConstructorFrame cFrame = frame.ForInstantiation(Type.AsClass(frame), Arguments);
			return cFrame.InvokeStep(frame.StackWith("Instantiation"));
		}
	}
}