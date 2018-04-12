using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
    public class TweedleArrayInitializer : TweedleExpression
    {
        private List<TweedleExpression> elements;

        public TweedleArrayInitializer(List<TweedleExpression> elements)
        {
            this.elements = elements;
        }

        public override TweedleValue Evaluate(TweedleFrame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}