namespace Alice.Tweedle
{
    /// <summary>
    /// Abstract type.
    /// </summary>
    public sealed class TAbstractNumberType : TAbstractType
    {
        public TAbstractNumberType(TAssembly inAssembly)
            : base(inAssembly, "Number")
        {
        }

        public override bool CanCast(TType inType)
        {
            return IsAssignableFrom(this, inType);
        }
    }
}