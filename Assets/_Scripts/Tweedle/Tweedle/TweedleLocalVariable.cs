namespace Alice.Tweedle
{
	public class TweedleLocalVariable : TweedleValueHolderDeclaration
	{
		public TweedleLocalVariable(TweedleType type, string name, TweedleExpression initializer)
			: base(type, name, initializer)
		{
		}

		public TweedleLocalVariable(TweedleType type, string name)
			: base(type, name)
		{
		}
	}
}