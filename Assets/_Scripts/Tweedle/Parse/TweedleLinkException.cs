using System;

namespace Alice.Tweedle.Parse
{
    public class TweedleLinkException : SystemException
    {
        public TweedleLinkException(string message)
            : base(message)
        {
        }
    }
}