using System.Collections.Generic;

namespace Alice.Linker
{
	[System.Serializable]
    public class AssetDescription
    {
        public ProjectIdentifier identifier;
		public List<ProjectIdentifier> prerequisites;
		public List<ResourceDescription> resources;

		public AssetDescription(AssetDescription asset)
		{
			this.identifier = asset.identifier;
			this.prerequisites = asset.prerequisites;
			this.resources = asset.resources;
		}
    }
}