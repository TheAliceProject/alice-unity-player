using System.Collections.Generic;

namespace Alice.Linker
{
	[System.Serializable]
    public class AssetDescription
    {
		public string Name
		{
			get
			{
				return description.name;
			}
		}

		public Description description;
		public Provenance provenance;
		public Package package;
		public List<ProjectIdentifier> prerequisites;
		public List<ResourceDescription> resources;

		public AssetDescription()
		{
		}

		public AssetDescription(AssetDescription asset)
		{
			this.description = asset.description;
			this.provenance = asset.provenance;
			this.package = asset.package;
			this.prerequisites = asset.prerequisites;
			this.resources = asset.resources;
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n--" + description.ToString();
			str += "\n--" + package.ToString();
			str += "\n--" + provenance.ToString();
			str += "\n-- prerequisites";
			for (int i = 0; i < prerequisites.Count; i++)
				str += "\n>" + prerequisites[i].ToString();
			str += "\n-- resources";
			for (int i = 0; i < resources.Count; i++)
				str += "\n>" + resources[i].ToString();
			return str + "\n";
		}
	}
}