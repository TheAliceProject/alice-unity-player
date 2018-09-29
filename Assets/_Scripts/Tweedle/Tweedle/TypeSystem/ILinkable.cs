namespace Alice.Tweedle
{
    /// <summary>
    /// Interface for objects to be linked during the linking step.
    /// </summary>
	public interface ILinkable
	{
        void Link(TAssemblyLinkContext inContext);
        void PostLink(TAssemblyLinkContext inContext);
    }
}