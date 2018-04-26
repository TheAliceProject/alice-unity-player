using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class ForEachInArrayLoop : AbstractLoop
	{
        private TweedleLocalVariable item;
		private TweedleExpression array;

        public ForEachInArrayLoop(TweedleLocalVariable item, TweedleExpression array, List<TweedleStatement> body) 
			: base(body)
		{
			this.item = item;
			this.array = array;
		}
	}
}