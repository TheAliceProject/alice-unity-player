using Alice.Tweedle.Parse;
using Alice.VM;

namespace Alice.Tweedle
{
	public interface ILinkable
	{
        void Link(TweedleSystem inSystem);
        void PostLink(TweedleSystem inSystem);
    }
}