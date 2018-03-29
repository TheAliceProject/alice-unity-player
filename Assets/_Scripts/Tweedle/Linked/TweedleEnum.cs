using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleEnum : TweedleType
    {
		private List<string> values;

        public TweedleEnum(string name, List<string> values) : base(name)
        {
			this.values = values;
        }

        public TweedleObject instantiate(VM.TweedleFrame frame, TweedleValue<TweedleType>[] args)
        {
            return null;
        }
    }
}