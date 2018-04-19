using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
    public class TweedleArrayInitializer : TweedleExpression
    {
        private List<TweedleExpression> elements;

        public TweedleArrayInitializer(TweedleArrayType arrayType, List<TweedleExpression> elements)
			: base(arrayType)
        {
            this.elements = elements;
        }

		public TweedleArrayInitializer(TweedleType elementType, List<TweedleExpression> elements)
			: base(new TweedleArrayType(elementType))
		{
			this.elements = elements;
		}

		public TweedleArrayInitializer(List<TweedleExpression> elements)
			: base(new TweedleArrayType(CommonType(elements)))
		{
			this.elements = elements;
		}

		private static TweedleType CommonType(List<TweedleExpression> elements)
		{
			return null;
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}