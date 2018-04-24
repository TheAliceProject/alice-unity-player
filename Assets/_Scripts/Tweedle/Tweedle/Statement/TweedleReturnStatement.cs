namespace Alice.Tweedle
{
	public class TweedleReturnStatement : TweedleStatement
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

        public TweedleReturnStatement()
		{
            this.expression = TweedleNull.NULL;
		}

		public TweedleReturnStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}
    }
}