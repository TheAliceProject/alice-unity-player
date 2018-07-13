using System;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ArrayIndexExpression : TweedleExpression
	{
		TweedleExpression array;
		TweedleExpression index;

		public ArrayIndexExpression(TweedleExpression array, TweedleExpression index)
			: base()
		{
			this.array = array;
			this.index = index;
		}

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			return new DoubleInputEvalStep(
				ToTweedle(),
				scope,
				array,
				index,
				(arr, val) => ((TweedleArray)arr)[val.ToInt()]);
		}

		internal override string ToTweedle()
		{
			return array.ToTweedle() + "[" + index.ToTweedle() + "]";
		}
	}
}