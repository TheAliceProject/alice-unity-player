using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class ClassInitializer : TweedleExpression
	{
		private Dictionary<string, TweedleExpression> arguments;

		public ClassInitializer(TweedleType type, Dictionary<string, TweedleExpression> arguments)
			: base(type)
		{
			this.arguments = arguments;
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}