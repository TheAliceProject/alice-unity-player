using System;

namespace Alice.Tweedle.Parse
{
    public class TweedleLinkException : TweedleRuntimeException
    {
        public TweedleLinkException(string message)
            : base(message)
        {
        }
    }
}