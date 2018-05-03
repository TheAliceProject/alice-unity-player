using System.Collections.Generic;

namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class ModelManifest : Manifest
    {
		public List<string> rootJoints;
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
	}
}