using System.Collections.Generic;

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
	}
}