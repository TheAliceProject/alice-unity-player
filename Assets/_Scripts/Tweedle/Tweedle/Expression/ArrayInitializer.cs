using System;
using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
	public class ArrayInitializer : TweedleExpression
	{
		List<TweedleExpression> elements;
		TweedleExpression initializeSize;

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

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			if (initializeSize != null)
			{
				initializeSize.Evaluate(frame, size =>
				{
					elements = new List<TweedleExpression>(size.ToInt());
				});
			}
			next(new TweedleArray((TweedleArrayType)this.Type,
			                      elements.Select(elem => elem?.EvaluateNow(frame)).ToList()));
		}
	}
}