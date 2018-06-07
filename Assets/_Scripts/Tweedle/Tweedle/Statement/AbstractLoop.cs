using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	abstract public class AbstractLoop : AbstractStatementWithBody
	{
        public AbstractLoop(List<TweedleStatement> body) : base(body)
		{
		}
	}
}