using System.Collections.Generic;

namespace Alice.Tweedle
{
	// public class TweedleLambdaType : TweedleType
	// {
	// 	List<TType> typeList;
	// 	TType result;

	// 	public TweedleLambdaType(List<TType> typeList, TType result)
	// 		: base("Lambda", result)
	// 	{
	// 		this.typeList = typeList;
	// 		this.result = result;
	// 	}

	// 	public override bool AcceptsType(TType type)
	// 	{
	// 		return this == type ||
	// 			((type is TweedleLambdaType) &&
	// 			 // TODO Enable this after LambdaExpressions are better typed
	// 			 // ResultTypesMatch(((TweedleLambdaType)type).result) &&
	// 			 ArgTypesMatch(typeList, ((TweedleLambdaType)type).typeList));
	// 	}

	// 	bool ResultTypesMatch(TType otherResult)
	// 	{
	// 		return (result == otherResult ||
	// 				(result != null && result.AcceptsType(otherResult)));
	// 	}

	// 	bool ArgTypesMatch(List<TType> listA, List<TType> listB)
	// 	{
	// 		if (listA == null || listB == null || listA.Count != listB.Count)
	// 		{
	// 			return false;
	// 		}
	// 		for (int i = 0; i < listA.Count; i++)
	// 		{
	// 			if (listA[i] != listB[i])
	// 			{
	// 				return false;
	// 			}
	// 		}
	// 		return true;
	// 	}
	// }
}