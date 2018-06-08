using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class Instantiation : TweedleExpression
	{
		private InvocableMethodHolder invocable;
		private Dictionary<string, TweedleExpression> arguments;

		public Instantiation(TweedleTypeReference type, Dictionary<string, TweedleExpression> arguments)
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