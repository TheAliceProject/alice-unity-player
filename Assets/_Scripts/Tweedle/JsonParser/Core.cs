namespace Alice.Json
{
	public class Json
	{
		string rootPath = "";
		public Json(string p)
		{
			rootPath = p;
		}

        public void Parse(Linker.Linker program, string json)
        {
			Linker.AssetDescription asset = UnityEngine.JsonUtility.FromJson<Linker.AssetDescription>(json);
			JSONObject jsonObj = new JSONObject(json);
			ParseResources(ref program, ref asset.resources, jsonObj[MemberInfoGetter.GetMemberName(() => asset.resources)]);
			Linker.ProjectType t = asset.package.identifier.Type;
            switch (t)
            {
                case Linker.ProjectType.Class:
					// Deprecated ClassAssetDescription
                    //Linker.ClassAssetDescription classAsset = new Linker.ClassAssetDescription(asset);
					// Parse JSON into a
					//program.AddClass(classAsset);
                    break;
                case Linker.ProjectType.Library:
                    Linker.LibraryDescription libAsset = new Linker.LibraryDescription(asset);
					// Parse JSON into a
					UnityEngine.Debug.Log(libAsset.ToString());
					program.AddLibrary(libAsset);
                    break;
                case Linker.ProjectType.World:
                    Linker.ProgramDescription worldAsset =  new Linker.ProgramDescription(asset);
					// Parse JSON into a
					program.AddProgram(worldAsset);
                    break;
                case Linker.ProjectType.Model:
					//Linker.ModelDescription modelAsset = new Linker.ModelDescription(asset);
					Linker.ModelDescription modelAsset = UnityEngine.JsonUtility.FromJson<Linker.ModelDescription>(json);
					UnityEngine.Debug.Log(modelAsset.ToString());
                    program.AddModel(modelAsset);
                    break;
            }
        }

		private void JsonType(JSONObject obj)
		{
			switch (obj.type)
			{
				case JSONObject.Type.OBJECT:
					for (int i = 0; i < obj.list.Count; i++)
					{
						string key = (string)obj.keys[i];
						JSONObject j = (JSONObject)obj.list[i];
						//Debug.Log(key);
						JsonType(j);
					}
					break;
				case JSONObject.Type.ARRAY:
					foreach (JSONObject j in obj.list)
					{
						JsonType(j);
					}
					break;
				case JSONObject.Type.STRING:
					//Debug.Log(obj.str);
					break;
				case JSONObject.Type.NUMBER:
					//Debug.Log(obj.n);
					break;
				case JSONObject.Type.BOOL:
					//Debug.Log(obj.b);
					break;
				case JSONObject.Type.NULL:
					//Debug.Log("NULL");
					break;
			}
		}

		private void ParseResources(
			ref Linker.Linker program,
			ref System.Collections.Generic.List<Linker.ResourceDescription> resources, 
			JSONObject json)
		{
			if (json == null || json.type != JSONObject.Type.ARRAY)
			{
				return;
			}
			// TODO do I want to add these resources to the program?
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
							Parse(program, subJson);
						}
						break;
					case Linker.Resource.ContentType.SkeletonMesh:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.StructureDescription>(json.list[i].ToString());
						break;
					case Linker.Resource.ContentType.Texture:
						resources[i] = UnityEngine.JsonUtility.FromJson<Linker.Resource.TextureDescription >(json.list[i].ToString());
						break;
				}
			}
		}
	}
}