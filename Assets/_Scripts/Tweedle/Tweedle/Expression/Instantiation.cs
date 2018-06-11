using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class Instantiation : TweedleExpression
	{
		Dictionary<string, TweedleExpression> Arguments { get; }

		public Instantiation(TweedleTypeReference type, Dictionary<string, TweedleExpression> arguments)
			: base(type)
		{
			Arguments = arguments;
			if (type == null)
			{
				UnityEngine.Debug.Log("Placeholder");
			}
		}

		public override void Evaluate(TweedleFrame frame)
		{
			ConstructorFrame cFrame = frame.ForInstantiation();
			cFrame.highestClass = Type.AsClass(frame);
			cFrame.instance = new TweedleObject(cFrame.highestClass);
			cFrame.highestClass
				.ConstructorWithArgs(Arguments)
				.Invoke(cFrame, Arguments);
		}
	}
}