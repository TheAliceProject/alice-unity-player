using System.Collections.Generic;

namespace Alice.Linker
{
    public class AssetDescription
    {
        public ProjectIdentifier Id
        {
            get { return id; }
        }

        private ProjectIdentifier id;
        private List<ResourceDescription> resources;
        private List<ProjectIdentifier> prerequisites;
    }
}