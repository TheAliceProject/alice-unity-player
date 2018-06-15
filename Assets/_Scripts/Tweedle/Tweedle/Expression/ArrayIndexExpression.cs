﻿using System;
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

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			throw new System.NotImplementedException();
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}