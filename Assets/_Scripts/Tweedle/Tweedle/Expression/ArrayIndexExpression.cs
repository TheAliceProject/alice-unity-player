using System;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ArrayIndexExpression : TweedleExpression
	{
		TweedleExpression array;
		TweedleExpression index;

		public ArrayIndexExpression(TweedleType type, TweedleExpression array, TweedleExpression index)
			: base(type)
		{
			this.array = array;
			this.index = index;
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			throw new NotImplementedException();
		}

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			throw new NotImplementedException();
		}
	}
}