using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleDoInOrder : TweedleAbstractStatementWithBody
	{
        public TweedleDoInOrder(List<TweedleStatement> body) : base(body)
		{
		}
	}
}