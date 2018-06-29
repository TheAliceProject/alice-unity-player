using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class AbsentPrimitiveStub : TweedlePrimitive
	{
		public AbsentPrimitiveStub(string absentNameSpace)
			: base(absentNameSpace, new List<TweedleField>(), new List<TweedleMethod>())
		{
		}
	}
}
