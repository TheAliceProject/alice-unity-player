using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleEachInArrayTogether : TweedleAbstractStatementWithBody
	{
        private TweedleLocalVariable itemVariable;

        public TweedleLocalVariable ItemVariable
        {
            get
            {
                return itemVariable;
            }
        }

        private TweedleExpression array;

        public TweedleExpression Array
        {
            get
            {
                return array;
            }
        }

        public TweedleEachInArrayTogether(TweedleLocalVariable itemVariable, TweedleExpression array, List<TweedleStatement> body) 
			: base(body)
		{
            this.itemVariable = itemVariable;
			this.array = array;
		}
	}
}