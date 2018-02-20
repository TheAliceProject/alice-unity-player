using System.Collections.Generic;

namespace Alice.Tweedle
{
    public class TweedleClass : TweedleType
    {
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public TweedleClass Super
        {
            get { return super; }
            set { super = value; }
        }

        private string name;
        private TweedleClass super;
        private Dictionary<string, TweedleProperty> properties;
        private Dictionary<string, TweedleMethod> methods;

        public TweedleClass(string name) : base()
        {
            
        }

        public TweedleObject instantiate(TweedleFrame frame, TweedleValue<TweedleType>[] args)
        {
            return null;
        }
    }
}