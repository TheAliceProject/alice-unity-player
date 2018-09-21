namespace Alice.Tweedle
{
	// public class TweedleArrayType : TweedleType
	// {
	// 	private TType valueType;

	// 	public TType ValueType
	// 	{
	// 		get { return valueType; }
	// 	}

	// 	public TweedleArrayType()
	// 		: base("[]")
	// 	{
	// 		this.valueType = null;
	// 	}

	// 	public TweedleArrayType(TType valueType)
	// 		: base(valueType?.Name + "[]")
	// 	{
	// 		this.valueType = valueType;
	// 	}

	// 	public override bool AcceptsType(TType type)
	// 	{
	// 		return this == type ||
	// 						((type is TweedleArrayType) &&
	// 						(valueType == null || valueType.AcceptsType(((TweedleArrayType)type).valueType)));
	// 	}
	// }
}