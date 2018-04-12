using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleConstructor: TweedleMethod
	{

        public TweedleConstructor(TweedleType type, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> body) 
			: base(type, name, required, optional, body)
		{
		}
	}
}