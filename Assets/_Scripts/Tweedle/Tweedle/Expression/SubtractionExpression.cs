﻿namespace Alice.Tweedle
{
	class SubtractionExpression : BinaryNumToNumExpression
	{

		public SubtractionExpression(TweedleExpression lhs, TweedleExpression rhs)
			: base(lhs, rhs)
		{
		}

		protected override int Evaluate(int left, int right)
		{
			return left - right;
		}

		protected override double Evaluate(double left, double right)
		{
			return left - right;
		}

		internal override string Operator()
		{
			return "-";
		}
	}
}