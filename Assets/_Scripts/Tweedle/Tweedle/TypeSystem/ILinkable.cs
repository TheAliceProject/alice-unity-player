namespace Alice.Tweedle
{
    /// <summary>
    /// Interface for objects to be linked during the linking step.
    /// </summary>
    public interface ILinkable
    {
        void Link(TAssembly inAssembly);
        void PostLink(TAssembly inAssembly);
    }
}