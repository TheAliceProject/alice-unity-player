﻿using System;
using System.Collections.Generic;
using Alice.Tweedle.Parse;

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

        public virtual void AddToSystem(TweedleSystem system) {
            throw new NotImplementedException("Only specialized subclasses can be added");
        }
    }
}