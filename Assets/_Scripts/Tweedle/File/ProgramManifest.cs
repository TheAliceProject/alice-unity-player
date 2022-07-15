using System.Collections.Generic;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    public class ProgramDescription : LibraryManifest
    {
        private List<Preference> preferences;
        //private List<Deprecated.StatementDescription> main; // TODO

        public ProgramDescription(Manifest asset) 
            : base(asset)
        {
        }

        public override void AddToSystem(TweedleSystem system) {
            system.AddProgram(this);
        }
    }
}