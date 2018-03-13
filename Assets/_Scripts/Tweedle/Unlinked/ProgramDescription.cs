using System.Collections.Generic;

namespace Alice.Tweedle.Unlinked
{
	public class ProgramDescription : LibraryDescription
    {
		private List<Preference> preferences;
        private List<Deprecated.StatementDescription> main;

		public ProgramDescription(AssetDescription asset) : base(asset)
		{
		}
	}
}