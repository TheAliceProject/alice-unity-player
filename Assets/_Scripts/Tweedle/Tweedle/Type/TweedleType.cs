namespace Alice.Tweedle
{
    abstract public class TweedleType
    {
		public string Name
		{
			get { return name; }
		}

		readonly string name;
		readonly TweedleType impliedType;

		public TweedleType(string name)
		{
			this.name = name;
			impliedType = null;
		}

		public TweedleType(string name, TweedleType impliedType)
		{
			this.name = name;
			this.impliedType = impliedType;
		}

        internal virtual double ValueToDouble(TweedleValue value)
        {
			throw new System.Exception("This type (" + this + ") cannot convert the value " + value +  "to a double.");
        }

		virtual public bool AcceptsType(TweedleType type)
		{
			return this == type || (type.impliedType != null && AcceptsType(type.impliedType));
		}
	}
}