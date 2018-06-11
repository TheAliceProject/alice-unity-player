namespace Alice.Tweedle
{
	public class TweedleTypeReference : TweedleType, InvocableMethodHolder
	{
		public TweedleTypeReference(string name) : base(name)
		{
		}

		internal override TweedleClass AsClass(TweedleFrame frame)
		{
			return frame.ClassNamed(Name);
		}

		internal override TweedleType AsDeclaredType(TweedleFrame frame)
		{
			return frame.TypeNamed(Name);
		}

		override public bool AcceptsType(TweedleType type)
		{
			if (this.Equals(type))
			{
				return true;
			}
			else
			{
				throw new TweedleLinkException("Attempt to use an unlinked type " + Name + ". To accept " + type);
			}
		}

		public TweedleMethod MethodNamed(string methodName)
		{
			throw new TweedleLinkException("Attempt to find the method " + methodName + " on an unlinked type " + Name);
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