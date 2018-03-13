namespace Alice.Tweedle.Unlinked
{
	public class JsonUnlinkedParser
	{
		private string rootPath = "";
		private UnlinkedSystem system;

		public JsonUnlinkedParser(string root)
		{
			this.system = new UnlinkedSystem();
			rootPath = root;
		}

		public JsonUnlinkedParser(UnlinkedSystem system, string root)
		{
			this.system = system;
			rootPath = root;
		}

		public void ParseFile(string json)
		{
			AssetDescription asset = UnityEngine.JsonUtility.FromJson<AssetDescription>(json);
			JSONObject jsonObj = new JSONObject(json);
			ParseResources(
				ref asset.resources, 
				jsonObj[MemberInfoGetter.GetMemberName(() => asset.resources)]
				);
			ProjectType t = asset.package.identifier.Type;
            switch (t)
            {
                case ProjectType.Library:
                    LibraryDescription libAsset = new LibraryDescription(asset);
					system.AddLibrary(libAsset);
					// REMOVE
					UnityEngine.Debug.Log(libAsset.ToString());
					break;
                case ProjectType.World:
                    ProgramDescription worldAsset =  new ProgramDescription(asset);
					system.AddProgram(worldAsset);
					// REMOVE
					UnityEngine.Debug.Log(worldAsset.ToString());
					break;
                case ProjectType.Model:
					ModelDescription modelAsset = UnityEngine.JsonUtility.FromJson<ModelDescription>(json);
					system.AddModel(modelAsset);
					// REMOVE
					UnityEngine.Debug.Log(modelAsset.ToString());
					break;
            }
        }

		private void ParseResources(
			ref System.Collections.Generic.List<ResourceDescription> resources, 
			JSONObject json)
		{
			if (json == null || json.type != JSONObject.Type.ARRAY)
			{
				return;
			}
			
			for (int i = 0; i < resources.Count; i++)
			{
				switch (resources[i].ContentType)
				{
					case Resource.ContentType.Audio:
						resources[i] = UnityEngine.JsonUtility.FromJson<Resource.AudioDescription>(json.list[i].ToString());
						break;
					case Resource.ContentType.Class:
						resources[i] = UnityEngine.JsonUtility.FromJson<Resource.ClassDescription>(json.list[i].ToString());

						break;
					case Resource.ContentType.Image:
						resources[i] = UnityEngine.JsonUtility.FromJson<Resource.ImageDescription>(json.list[i].ToString());
						break;
					case Resource.ContentType.Model:
						resources[i] = UnityEngine.JsonUtility.FromJson<Resource.ModelDescription>(json.list[i].ToString());
						for (int j = 0; j < resources[i].files.Count; j++)
						{
							string relativePath = System.IO.Path.Combine(rootPath, resources[i].files[j]);
							string subJson = System.IO.File.ReadAllText(relativePath);
							ParseFile(subJson);
						}
						break;
					case Resource.ContentType.SkeletonMesh:
						resources[i] = UnityEngine.JsonUtility.FromJson<Resource.StructureDescription>(json.list[i].ToString());
						break;
					case Resource.ContentType.Texture:
						resources[i] = UnityEngine.JsonUtility.FromJson<Resource.TextureDescription >(json.list[i].ToString());
						break;
				}
				system.AddResource(resources[i]);
			}
		}
	}
}