using System;

namespace Alice.Tweedle.Parse
{
    public class TweedleParseException : SystemException
    {
        public string ExpectedVersion;
        public string DiscoveredVersion;
        public TweedleParseException(string message, string expected, string actual)
            : base(message)
        {
            ExpectedVersion = expected;
            DiscoveredVersion = actual;
        }
    }
}