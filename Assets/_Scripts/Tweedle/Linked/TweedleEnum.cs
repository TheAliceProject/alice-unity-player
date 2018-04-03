using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleEnum : TweedleType
    {
		public List<string> Values
		{
			get { return values; }
		}

		private List<string> values;

        public TweedleEnum(string name, List<string> values) 
			: base(name)
        {
			this.values = values;
        }

        public TweedleObject instantiate(VM.TweedleFrame frame, TweedleValue[] args)
        {
            return null;
        }
    }
}