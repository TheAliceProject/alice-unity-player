using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleLambdaType : TweedleType
	{
		List<TweedleType> typeList;
		TweedleType result;

		public TweedleLambdaType(List<TweedleType> typeList, TweedleType result)
			: base("Lambda", result)
		{
			this.typeList = typeList;
			this.result = result;
		}

		public override bool AcceptsType(TweedleType type)
		{
			return this == type ||
				((type is TweedleLambdaType) &&
				 // TODO Enable this after LambdaExpressions are better typed
				 // ResultTypesMatch(((TweedleLambdaType)type).result) &&
				 ArgTypesMatch(typeList, ((TweedleLambdaType)type).typeList));
		}

		bool ResultTypesMatch(TweedleType otherResult)
		{
			return (result == otherResult ||
					(result != null && result.AcceptsType(otherResult)));
		}

		bool ArgTypesMatch(List<TweedleType> listA, List<TweedleType> listB)
		{
			if (listA == null || listB == null || listA.Count != listB.Count)
			{
				return false;
			}
			for (int i = 0; i < listA.Count; i++)
			{
				if (listA[i] != listB[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}