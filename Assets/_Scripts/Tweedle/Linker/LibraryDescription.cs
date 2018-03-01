using System.Collections.Generic;

namespace Alice.Linker
{
    public class LibraryDescription : AssetDescription
    {
        private string name;
        private List<ClassDescription> classes;
        private List<ModelDescription> models;

		public LibraryDescription(AssetDescription asset) : base(asset)
		{
		}
	}
}