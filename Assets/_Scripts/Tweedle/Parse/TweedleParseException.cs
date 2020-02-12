using System;

namespace Alice.Tweedle.Parse
{
    public class TweedleParseException : SystemException
    {
        public TweedleParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}