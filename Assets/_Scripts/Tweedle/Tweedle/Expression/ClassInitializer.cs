using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class ClassInitializer : TweedleExpression
	{
		private InvocableMethodHolder invocable;
		private Dictionary<string, TweedleExpression> arguments;

		public ClassInitializer(TweedleTypeReference type, Dictionary<string, TweedleExpression> arguments)
			: base(type)
		{
			this.invocable = type;
			this.arguments = arguments;
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}