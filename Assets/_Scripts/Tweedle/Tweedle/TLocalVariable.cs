namespace Alice.Tweedle
{
	public class TLocalVariable : TValueHolderDeclaration
	{
		public TLocalVariable(TTypeRef type, string name, ITweedleExpression initializer)
			: base(type, name, initializer)
		{
		}

		public TLocalVariable(TTypeRef type, string name)
			: base(type, name)
		{
		}
	}
}