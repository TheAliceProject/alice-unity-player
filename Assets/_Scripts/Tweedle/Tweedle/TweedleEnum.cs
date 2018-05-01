using System.Collections.Generic;
using System.Linq;

namespace Alice.Tweedle
{
    public class TweedleEnum : TweedleType
    {
		//private List<string> values;
		private Dictionary<string, TweedleMethod> values;

		public List<string> Values
		{
			get { return values.Keys.ToList(); }
		}

        public TweedleEnum(string name, Dictionary<string, TweedleMethod> values) 
			: base(name)
        {
			this.values = values;
        }

		public TweedleObject Instantiate(TweedleFrame frame, TweedleValue[] args)
        {
            return null;
        }
    }
}