using System.Collections.Generic;

namespace Alice.Linker
{
	[System.Serializable]
	public class ModelDescription
    {
		public string name;
		public string creationYear;
		public string creator;
		public string icon;
		public Resource.BoundingBox boundingBox;
		public List<string> tags;
		public List<string> groupTags;
		public List<string> themeTags;
		public string extends;
		public List<string> rootJoints;
		public List<string> additionalJoints;
		public List<string> additionalJointArrays;
		public List<string> poses;
		public List<Resource.ResourceSet> textureSets;
		public List<Resource.ModelSet> models;
		public List<Resource.TexturedModelSet> texturedModels;
    }
}
