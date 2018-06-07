﻿namespace Alice.Tweedle
{
	public class ExpressionStatement : TweedleStatement
	{
		private TweedleExpression expression;

        public TweedleExpression Expression
        {
            get
            {
                return expression;
            }
        }

        public ExpressionStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}

		public override void Execute(TweedleFrame frame)
		{
			expression.Evaluate(frame);
		}
	}
}