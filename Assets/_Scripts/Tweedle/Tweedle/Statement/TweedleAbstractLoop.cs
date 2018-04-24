using System.Collections.Generic;

namespace Alice.Tweedle
{
	abstract public class TweedleAbstractLoop : TweedleAbstractStatementWithBody
	{
        public TweedleAbstractLoop(List<TweedleStatement> body) : base(body)
		{
		}
	}
}