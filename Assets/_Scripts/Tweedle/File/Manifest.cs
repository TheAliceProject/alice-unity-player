﻿using System.Collections.Generic;

namespace Alice.Tweedle.File
{
	[System.Serializable]
    public class Manifest
    {
		public ProjectIdentifier Identifier
		{
			get { return metadata.identifier; }
		}

		public string Name
		{
			get { return description.name; }
		}

		public Description description;
		public Provenance provenance;
		public MetaData metadata;
		public List<ProjectIdentifier> prerequisites;
		public List<ResourceReference> resources;

		public Manifest() {}

		public Manifest(Manifest asset)
		{
			this.description = asset.description;
			this.provenance = asset.provenance;
			this.metadata = asset.metadata;
			this.prerequisites = asset.prerequisites;
			this.resources = asset.resources;
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n--" + description.ToString();
			str += "\n--" + metadata.ToString();
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