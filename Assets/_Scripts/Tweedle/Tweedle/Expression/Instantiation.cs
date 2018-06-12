using System;
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

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			ConstructorFrame cFrame = frame.ForInstantiation(Type.AsClass(frame), next);
			cFrame.Instantiate(Arguments);
		}
	}
}