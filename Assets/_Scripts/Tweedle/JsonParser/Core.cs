namespace Alice.Json
{
	public class Json
	{
        public void Parse(Linker.Linker program, string json)
        {
			Linker.AssetDescription asset = UnityEngine.JsonUtility.FromJson<Linker.AssetDescription>(json);
			JSONObject jsonObj = new JSONObject(json);
            Linker.ProjectType t = asset.identifier.Type; // read in json
            switch (t)
            {
                case Linker.ProjectType.Class:
                    Linker.ClassAssetDescription classAsset = new Linker.ClassAssetDescription(asset);
					classAsset = (Linker.ClassAssetDescription)asset;
					// Parse JSON into a
					program.AddClass(classAsset);
                    break;
                case Linker.ProjectType.Library:
                    Linker.LibraryDescription libAsset = new Linker.LibraryDescription(asset);
					libAsset = (Linker.LibraryDescription)asset;
					// Parse JSON into a
					program.AddLibrary(libAsset);
                    break;
                case Linker.ProjectType.World:
                    Linker.ProgramDescription worldAsset =  new Linker.ProgramDescription(asset);
					worldAsset = (Linker.ProgramDescription)asset;
					// Parse JSON into a
					program.AddProgram(worldAsset);
                    break;
                case Linker.ProjectType.Model:
					Linker.ModelAssetDescription modelAsset = new Linker.ModelAssetDescription(asset);
					modelAsset.Description = UnityEngine.JsonUtility.FromJson<Linker.ModelDescription>(json);
					UnityEngine.Debug.Log(modelAsset.Description.name);
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
	}
}