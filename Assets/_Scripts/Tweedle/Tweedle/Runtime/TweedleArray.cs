using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleArray : TweedleValue
    {
		readonly TweedleArrayType arrayType;
		readonly List<TweedleValue> values;

        public int Length
        {
            get { return values.Count; }
        }

        public TweedleValue this[int i]
        {
            get { return values[i]; }
        }

        public TweedleArray(TweedleArrayType arrayType, List<TweedleValue> values)
			: base(arrayType)
        {
            this.values = values;
			this.arrayType = arrayType;
        }
	}
}