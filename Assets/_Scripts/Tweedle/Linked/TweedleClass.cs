using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleClass : TweedleType
    {
        public TweedleClass Super
        {
            get { return super; }
            set { super = value; }
        }

        private TweedleClass super;
        private Dictionary<string, TweedleProperty> properties;
        private Dictionary<string, TweedleMethod> methods;

        public TweedleClass(string name) : base(name)
        {
        }

        public TweedleObject instantiate(VM.TweedleFrame frame, TweedleValue<TweedleType>[] args)
        {
            return null;
        }
    }
}