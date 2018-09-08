namespace Alice.Tweedle
{
    /// <summary>
    /// Abstract type.
    /// </summary>
    public sealed class TAbstractNumberType : TAbstractType
    {
        public TAbstractNumberType()
            : base("Number")
        {
        }

        public override bool CanCast(TType inType)
        {
            return IsAssignableFrom(this, inType);
        }
    }
}