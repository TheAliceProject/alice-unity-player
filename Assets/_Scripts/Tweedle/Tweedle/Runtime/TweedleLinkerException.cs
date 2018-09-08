using System;

namespace Alice.Tweedle
{
    public class TweedleLinkerException : SystemException
    {
        public TweedleLinkerException(string message)
			:base(message)
        {
        }
    }
}