using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleConstructor: TweedleMethod
	{
		private List<TweedleField> required;
		private List<TweedleField> optional;

		public TweedleConstructor(TweedleType type, string name, List<TweedleField> required, List<TweedleField> optional, List<TweedleStatement> body) 
			: base(type, name, required, optional, body)
		{
			this.required = required;
			this.optional = optional;
		}
	}
}