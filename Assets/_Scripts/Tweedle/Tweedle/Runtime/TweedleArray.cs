using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleArray : TweedleValue
	{
		readonly TweedleArrayType arrayType;
		public List<TweedleValue> Values { get; }

		public int Length
		{
			get { return Values.Count; }
		}

		public TweedleValue this[int i]
		{
			get { return Values[i]; }
		}

		public TweedleArray(TweedleArrayType arrayType, List<TweedleValue> values)
			: base(arrayType)
		{
			Values = values;
			this.arrayType = arrayType;
		}
	}
}