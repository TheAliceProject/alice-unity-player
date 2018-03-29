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

		public ModelManifest() : base()
		{
		}

		public ModelManifest(Manifest asset) : base(asset)
		{
		}

		public override string ToString()
		{
			string str = base.ToString();
			str += "\n-- root joints";
			for (int i = 0; i < rootJoints.Count; i++)
				str += "\n>" + rootJoints[i].ToString();
			str += "\n-- additional joints";
			for (int i = 0; i < additionalJoints.Count; i++)
				str += "\n>" + additionalJoints[i].ToString();
			str += "\n-- additional joint arrays";
			for (int i = 0; i < additionalJointArrays.Count; i++)
				str += "\n>" + additionalJointArrays[i].ToString();
			str += "\n-- poses";
			for (int i = 0; i < poses.Count; i++)
				str += "\n>" + poses[i].ToString();
			str += "\n--" + boundingBox.ToString();
			str += "\n-- texture sets";
			for (int i = 0; i < textureSets.Count; i++)
				str += "\n>" + textureSets[i].ToString();
			str += "\n-- structures";
			for (int i = 0; i < structures.Count; i++)
				str += "\n>" + structures[i].ToString();
			str += "\n-- models";
			for (int i = 0; i < models.Count; i++)
				str += "\n>" + models[i].ToString();
			return str;
		}
	}
}