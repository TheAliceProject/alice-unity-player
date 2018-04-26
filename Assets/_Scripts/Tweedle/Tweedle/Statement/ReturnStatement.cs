namespace Alice.Tweedle
{
	public class ReturnStatement : TweedleStatement
	{
		private TweedleExpression expression;

        public TweedleExpression Expression
        {
            get
            {
                return expression;
            }
        }

        public TweedleType Type
        {
            get
            {
                return expression.Type;
            }
        }

        public ReturnStatement()
		{
            this.expression = TweedleNull.NULL;
		}

		public ReturnStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}
    }
}