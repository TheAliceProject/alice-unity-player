﻿namespace Alice.Tweedle
{
    public abstract class TweedleValue : TweedleExpression
    {
		protected TweedleValue(TweedleType type)
			: base(type)
		{
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			return this;
		}
	}
}
