using System.Collections.Generic;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    public class LibraryManifest : Manifest
    {
        public LibraryManifest(Manifest asset) : base(asset)
        {
        }

        public override void AddToSystem(TweedleSystem system) {
            system.AddLibrary(this);
        }
    }
}