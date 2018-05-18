namespace Alice.Tweedle
{
    public abstract class TweedleExpression
    {
		public TweedleType Type
		{
			get { return type; }
		}

		TweedleType type;

		protected TweedleExpression()
		{
			this.type = null;
        }

        protected TweedleExpression(TweedleType type)
        {
            this.type = type;
        }

        public abstract TweedleValue Evaluate(TweedleFrame frame);

		internal virtual bool IsLiteral()
		{
			return false;
		}
	}
}