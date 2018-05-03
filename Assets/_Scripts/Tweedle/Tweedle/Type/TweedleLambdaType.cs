using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleLambdaType : TweedleType
	{
		private List<TweedleType> typeList;
		private TweedleType result;

        public TweedleLambdaType(List<TweedleType> typeList, TweedleType result) 
            : base("TODO", result) // TODO what is the name
		{
			this.typeList = typeList;
			this.result = result;
		}
    }
}