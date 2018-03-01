using System.Collections.Generic;

namespace Alice.Linker
{
    public class ProgramDescription : LibraryDescription
    {
        private List<StatementDescription> main;

		public ProgramDescription(AssetDescription asset) : base(asset)
		{
		}
	}
}