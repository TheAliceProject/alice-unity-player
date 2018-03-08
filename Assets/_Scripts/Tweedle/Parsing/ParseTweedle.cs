namespace Alice.Parse
{
	public class ParseTweedle
	{
		private string rootPath = "";
		private Linker.Linker program;

		public ParseTweedle(Linker.Linker program, string p)
		{
			this.program = program;
			rootPath = p;
		}

		public void JsonFile(string json)
		{
			Linker.AssetDescription asset = UnityEngine.JsonUtility.FromJson<Linker.AssetDescription>(json);
			JSONObject jsonObj = new JSONObject(json);
			JsonResources(
				ref asset.resources, 
				jsonObj[MemberInfoGetter.GetMemberName(() => asset.resources)]
				);
			Linker.ProjectType t = asset.package.identifier.Type;
            switch (t)
            {
                case Linker.ProjectType.Library:
                    Linker.LibraryDescription libAsset = new Linker.LibraryDescription(asset);
					program.AddLibrary(libAsset);
					// REMOVE
					UnityEngine.Debug.Log(libAsset.ToString());
					break;
                case Linker.ProjectType.World:
                    Linker.ProgramDescription worldAsset =  new Linker.ProgramDescription(asset);
					program.AddProgram(worldAsset);
					// REMOVE
					UnityEngine.Debug.Log(worldAsset.ToString());
					break;
                case Linker.ProjectType.Model:
					Linker.ModelDescription modelAsset = UnityEngine.JsonUtility.FromJson<Linker.ModelDescription>(json);
                    program.AddModel(modelAsset);
					// REMOVE
					UnityEngine.Debug.Log(modelAsset.ToString());
					break;
            }
        }

		private void JsonResources(
			ref System.Collections.Generic.List<Linker.ResourceDescription> resources, 
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
					case Linker.Resource.ContentType.Audio:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.AudioDescription>(json.list[i].ToString());
						break;
					case Linker.Resource.ContentType.Class:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.ClassDescription>(json.list[i].ToString());

						break;
					case Linker.Resource.ContentType.Image:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.ImageDescription>(json.list[i].ToString());
						break;
					case Linker.Resource.ContentType.Model:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.ModelDescription>(json.list[i].ToString());
						for (int j = 0; j < resources[i].files.Count; j++)
						{
							string relativePath = System.IO.Path.Combine(rootPath, resources[i].files[j]);
							string subJson = System.IO.File.ReadAllText(relativePath);
							JsonFile(subJson);
						}
						break;
					case Linker.Resource.ContentType.SkeletonMesh:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.StructureDescription>(json.list[i].ToString());
						break;
					case Linker.Resource.ContentType.Texture:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.TextureDescription >(json.list[i].ToString());
						break;
				}
				program.AddResource(resources[i]);
			}
		}
	}
}