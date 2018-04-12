namespace Alice.Tweedle
{
    public class TweedleOptionalParameter : TweedleValueHolderDeclaration
    {
        private TweedleExpression initializer;

        public TweedleOptionalParameter(TweedleType type, string name, TweedleExpression initializer)
            : base(type, name)
        {
            this.initializer = initializer;
        }
	}
}