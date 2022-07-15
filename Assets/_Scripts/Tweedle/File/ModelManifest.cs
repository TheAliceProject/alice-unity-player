using System.Collections.Generic;
using System.Linq;
using Alice.Tweedle.Parse;
using Alice.Utils;
using UnityEngine;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ModelManifest : Manifest
    {
        public List<string> rootJoints;
        public List<JointBounds> jointBounds;
        public List<Joint> additionalJoints;
        public List<Joint> additionalJointArrays;
        public List<string> poses; // TODO
        public BoundingBox boundingBox;
        public List<TextureReference> textureSets;
        public List<StructureReference> structures;
        public List<ModelVariant> models;

        public ModelManifest() 
            : base()
        {
        }

        public ModelManifest(Manifest asset) 
            : base(asset)
        {
        }

        public StructureReference GetStructure(string structureName)
        {
            return resources.FirstOrDefault(resource => resource.name == structureName) as StructureReference;
        }

        public override void AddToSystem(TweedleSystem system) {
            system.AddModel(this);
        }
    }
}