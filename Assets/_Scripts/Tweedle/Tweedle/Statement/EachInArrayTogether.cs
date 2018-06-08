using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class EachInArrayTogether : AbstractStatementWithBody
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

        public EachInArrayTogether(TweedleLocalVariable itemVariable, TweedleExpression array, List<TweedleStatement> body) 
			: base(body)
		{
            this.itemVariable = itemVariable;
			this.array = array;
		}
	}
}