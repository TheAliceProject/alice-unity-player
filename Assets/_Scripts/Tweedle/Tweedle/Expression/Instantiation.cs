using System;
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
			return new ConstructorStep(Type.AsClass(frame), frame, this);
		}
	}

	internal class ConstructorStep : EvaluationStep // MethodStep?
	{
		private TweedleClass tweedleClass;
		private TweedleFrame frame;
		private Instantiation instantiation;

		public ConstructorStep(TweedleClass tweedleClass, TweedleFrame frame, Instantiation instantiation)
		{
			this.tweedleClass = tweedleClass;
			this.frame = frame;
			this.instantiation = instantiation;
		}

		internal override bool Execute()
		{
			ConstructorFrame cFrame = frame.ForInstantiation(tweedleClass, null);//next);
			cFrame.Instantiate(instantiation.Arguments);
			return false;
		}
	}
}