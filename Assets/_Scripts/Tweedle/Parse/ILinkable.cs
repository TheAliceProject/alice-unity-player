using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public interface ILinkable
	{
        void Link(TweedleSystem inSystem);
        void PostLink(TweedleSystem inSystem);
    }
}