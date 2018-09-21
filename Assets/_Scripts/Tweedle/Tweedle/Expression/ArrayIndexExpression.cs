using System;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public class ArrayIndexExpression : TweedleExpression
	{
		ITweedleExpression array;
		ITweedleExpression index;

		public ArrayIndexExpression(ITweedleExpression array, ITweedleExpression index)
			: base()
		{
			this.array = array;
			this.index = index;
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			return new TwoValueComputationStep(
				ToTweedle(),
				scope,
				array,
				index,
				(arr, val) => (arr.Array())[val.ToInt()]);
		}

		public override string ToTweedle()
		{
			return array.ToTweedle() + "[" + index.ToTweedle() + "]";
		}
	}
}