namespace Alice.Tweedle
{
	public class TweedleTypeReference : TweedleType, InvocableMethodHolder
	{
		public TweedleTypeReference(string name) : base(name)
		{
        }

        override public bool AcceptsType(TweedleType type)
        {
            if (this.Equals(type))
            {
                return true;
            }
            else
            {
                throw new TweedleLinkException("LinkException - Attempt to use an unlinked type " + Name);
            }
        }

		public void Invoke(TweedleFrame frame, TweedleObject target, TweedleMethod method, TweedleValue[] arguments)
		{
			throw new TweedleLinkException("Attempt to invoke the method " + method.Name + " on an unlinked type " + Name);
		}

        override public bool Equals(object obj)
        {
            return obj is TweedleTypeReference && this.Name.Equals(((TweedleTypeReference)obj).Name);
        }

        override public int GetHashCode()
        {
            return Name.GetHashCode();
        }
	}
}