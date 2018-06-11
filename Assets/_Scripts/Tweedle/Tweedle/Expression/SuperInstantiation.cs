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

		public override void Evaluate(TweedleFrame frame)
		{
			ConstructorFrame cFrame = frame.ForInstantiation();
			TweedleConstructor superConst = null;
			while (superConst == null && cFrame.highestClass != null)
			{
				TweedleClass supererClass = cFrame.highestClass.SuperClass(cFrame);
				cFrame.highestClass = supererClass;
				superConst = supererClass?.ConstructorWithArgs(Arguments);
			}
			if (superConst != null)
			{
				superConst.Invoke(cFrame, Arguments);
			}
			else
			{
				cFrame.Next();
			}
		}
	}
}