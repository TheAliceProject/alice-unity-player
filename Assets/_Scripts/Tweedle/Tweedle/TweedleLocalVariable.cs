namespace Alice.Tweedle
{
    public class TweedleLocalVariable : TweedleValueHolderDeclaration
    {
        private TweedleExpression initializer;

        public TweedleExpression Initializer
        {
            get
            {
                return initializer;
            }
        }

        public TweedleLocalVariable(TweedleType type, string name, TweedleExpression initializer)
            : base(type, name)
        {
            this.initializer = initializer;
        }

        public TweedleLocalVariable(TweedleType type, string name)
            : base(type, name)
        {
            this.initializer = null;
        }
    }
}