namespace Alice.Tweedle
{
	public class TweedleTypeReference : TweedleType
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
                throw new System.Exception("LinkException - Attempt to use an unlinked type " + Name);
            }
        }

        /*override public void invoke(Frame frame, TweedleObject target, TweedleMethod method, TweedleValue[] arguments)
        {
            throw new System.Exception("LinkException - Attempt to invoke the method " + method.Name + " on an unlinked type " + Name);
        }*/

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