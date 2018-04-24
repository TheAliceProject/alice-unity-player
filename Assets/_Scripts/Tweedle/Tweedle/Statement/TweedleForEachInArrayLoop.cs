using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleForEachInArrayLoop : TweedleAbstractLoop
	{
        private TweedleLocalVariable item;
		private TweedleExpression array;

        public TweedleForEachInArrayLoop(TweedleLocalVariable item, TweedleExpression array, List<TweedleStatement> body) 
			: base(body)
		{
			this.item = item;
			this.array = array;
		}
	}
}