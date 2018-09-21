namespace Alice.Tweedle
{
    public class TParameter : TValueHolderDeclaration
    {
        public readonly bool Required;

        private TParameter(TTypeRef inType, string inName)
            : base(inType, inName)
		{
            Required = true;
        }

        private TParameter(TTypeRef inType, string inName, ITweedleExpression inInitializer)
            : base(inType, inName, inInitializer)
		{
            Required = false;
        }

        static public TParameter RequiredParameter(TTypeRef inType, string inName)
        {
            return new TParameter(inType, inName);
        }

        static public TParameter OptionalParameter(TTypeRef inType, string inName, ITweedleExpression inInitializer)
        {
            return new TParameter(inType, inName, inInitializer);
        }

        static public readonly TParameter[] EMPTY_PARAMS = new TParameter[0];
    }
}