using System;

namespace Alice.Tweedle.Parse
{
    public class TweedleVersionException : SystemException
    {
        public string ExpectedVersion;
        public string DiscoveredVersion;
        public TweedleVersionException(string message, string expected, string actual)
            : base(message)
        {
            ExpectedVersion = expected;
            DiscoveredVersion = actual;
        }
    }
}