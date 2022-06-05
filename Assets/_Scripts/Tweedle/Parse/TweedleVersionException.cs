using System;

namespace Alice.Tweedle.Parse
{
    public class TweedleVersionException : SystemException
    {
        public readonly string ExpectedVersion;
        public readonly string DiscoveredVersion;
        public readonly string PlayerCompatibleAliceVersion;
        public readonly string SourceAliceVersion;
        public readonly int LibraryComparison;
        public TweedleVersionException(string message, string playerLibrary, string requestedLibrary,
            string playerCompatibleAlice, string sourceAlice, int libraryComparison)
            : base(message)
        {
            ExpectedVersion = playerLibrary;
            DiscoveredVersion = requestedLibrary;
            PlayerCompatibleAliceVersion = playerCompatibleAlice;
            SourceAliceVersion = sourceAlice;
            LibraryComparison = libraryComparison;
        }
    }
}