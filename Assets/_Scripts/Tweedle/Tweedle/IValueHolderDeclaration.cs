using Alice.VM;

namespace Alice.Tweedle
{
	public interface IValueHolderDeclaration
	{
		string Name { get; }
		TTypeRef Type { get; }
        string ToTweedle();
	}

	static public class IValueHolderDeclarationExtensions
	{
		static public bool Accepts(this IValueHolderDeclaration inDecl, ExecutionScope inScope, TValue inValue)
		{
        	return inValue.Type.CanCast(inDecl.Type.Get(inScope));
    	}
	}
}