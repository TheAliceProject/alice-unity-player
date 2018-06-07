using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
    public class ArrayInitializer : TweedleExpression
    {
        private List<TweedleExpression> elements;
		private TweedleExpression initializeSize;

        public ArrayInitializer(TweedleArrayType arrayType, List<TweedleExpression> elements)
			: base(arrayType)
        {
            this.elements = elements;
        }

		public ArrayInitializer(TweedleType elementType, List<TweedleExpression> elements)
			: base(new TweedleArrayType(elementType))
		{
			this.elements = elements;
		}

		public ArrayInitializer(List<TweedleExpression> elements)
			: base(new TweedleArrayType(CommonType(elements)))
		{
			this.elements = elements;
		}

		public ArrayInitializer(TweedleArrayType arrayType, TweedleExpression initializeSize)
			: base(arrayType)
		{
			this.initializeSize = initializeSize;
		}

		private static TweedleType CommonType(List<TweedleExpression> elements)
		{
			return null;
		}

		public override void Evaluate(TweedleFrame frame)
        {
			if (initializeSize != null)
			{
				// TODO update and restore
				//this.elements = new List<TweedleExpression>(((TweedlePrimitiveValue<int>)initializeSize.Evaluate(frame)).Value);
			}
			// TODO update and restore
			//return new TweedleArray(
				//(TweedleArrayType)this.Type
				//elements.Select(elem => elem?.Evaluate(frame)).ToList()
				//);
        }
    }
}