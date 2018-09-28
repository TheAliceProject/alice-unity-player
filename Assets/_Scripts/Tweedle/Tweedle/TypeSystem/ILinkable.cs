namespace Alice.Tweedle
{
	public interface ILinkable
	{
        void Link(TAssembly[] inAssemblies);
        void PostLink(TAssembly[] inAssemblies);
    }
}