using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class AbsentPrimitiveClassStub : TweedlePrimitiveClass
	{
		public AbsentPrimitiveClassStub(string absentNameSpace)
			: base(absentNameSpace, new List<TweedleField>(), new List<TweedleMethod>())
		{
		}
	}
}
