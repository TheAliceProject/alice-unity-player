namespace Alice.Tweedle
{
	public class TypeValue : TweedleValue
	{
		TweedleTypeDeclaration type;

		public TypeValue(TweedleClass type)
			: base(null) // Until there is a metaclass to replace null
		{
			this.type = type;
		}

		internal override TweedleMethod MethodNamed(TweedleFrame frame, string methodName)
		{
			TweedleMethod method = type.MethodNamed(frame, methodName);
			if (method.IsStatic())
			{
				return method;
			}
			return null;
		}
	}
}