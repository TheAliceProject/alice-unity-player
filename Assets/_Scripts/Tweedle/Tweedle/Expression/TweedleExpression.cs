namespace Alice.Tweedle
{
    public abstract class TweedleExpression
    {
		public TweedleType Type
		{
			get { return type; }
		}

		private TweedleType type;

		protected TweedleExpression(TweedleType type)
		{
			this.type = type;
		}

        public abstract TweedleValue Evaluate(VM.TweedleFrame frame);
    }
}