namespace Alice.Tweedle
{
	class PrefixIncrementExpression : PrePostfixExpression
	{

		public PrefixIncrementExpression(TweedleExpression expression)
			: base(expression)
		{
		}

		public override TweedleValue Evaluate(VM.TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}