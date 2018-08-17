using System;

namespace Alice.Tweedle
{
    public class TweedleRuntimeException : SystemException
    {
        public TweedleRuntimeException(string message)
			:base(message)
        {
        }
    }
}