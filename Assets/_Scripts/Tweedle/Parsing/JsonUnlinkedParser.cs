using Alice.Tweedle.File;
using System.Collections.Generic;

namespace Alice.Tweedle.Unlinked
{
	public class JsonUnlinkedParser
	{
		private string rootPath = "";
		private UnlinkedSystem system;
		private TweedleUnlinkedParser tweedleParser;

		public JsonUnlinkedParser(string root)
		{
			this.system = new UnlinkedSystem();
			this.rootPath = root;
			this.tweedleParser = new TweedleUnlinkedParser();
		}

		public JsonUnlinkedParser(UnlinkedSystem system, string root)
		{
			this.system = system;
			this.rootPath = root;
			this.tweedleParser = new TweedleUnlinkedParser();
		}

		public void ParseFile(string json)
		{
			Manifest asset = UnityEngine.JsonUtility.FromJson<Manifest>(json);
			JSONObject jsonObj = new JSONObject(json);
			ParseResources(
				asset.resources, 
				jsonObj[MemberInfoGetter.GetMemberName(() => asset.resources)]
				);
			ProjectType t = asset.Identifier.Type;
            switch (t)
            {
                case ProjectType.Library:
                    LibraryManifest libAsset = new LibraryManifest(asset);
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
					ModelManifest modelAsset = UnityEngine.JsonUtility.FromJson<ModelManifest>(json);
					system.AddModel(modelAsset);
					// REMOVE
					UnityEngine.Debug.Log(modelAsset.ToString());
					break;
			}
        }

		private void ParseResources(
			List<ResourceReference> resources, 
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
					case ContentType.Audio:
						resources[i] = UnityEngine.JsonUtility.FromJson<AudioReference>(json.list[i].ToString());
						break;
					case ContentType.Class:
						resources[i] = UnityEngine.JsonUtility.FromJson<ClassReference>(json.list[i].ToString());
						for (int j = 0; j < resources[i].files.Count; j++)
						{
							string absPath = System.IO.Path.Combine(rootPath, resources[i].files[j]);
							TweedleClass tweClass = (TweedleClass)tweedleParser.Parse(System.IO.File.ReadAllText(absPath));
							UnityEngine.Debug.Log(tweClass.ToString());
							system.AddClass(tweClass);
						}
						break;
					case ContentType.Enum:
						resources[i] = UnityEngine.JsonUtility.FromJson<EnumReference>(json.list[i].ToString());
						for (int j = 0; j < resources[i].files.Count; j++)
						{
							string absPath = System.IO.Path.Combine(rootPath, resources[i].files[j]);
							TweedleEnum tweEnum = (TweedleEnum)tweedleParser.Parse(System.IO.File.ReadAllText(absPath));
							system.AddEnum(tweEnum);
						}
						break;
					case ContentType.Image:
						resources[i] = UnityEngine.JsonUtility.FromJson<ImageReference>(json.list[i].ToString());
						break;
					case ContentType.Model:
						resources[i] = UnityEngine.JsonUtility.FromJson<ModelReference>(json.list[i].ToString());
						for (int j = 0; j < resources[i].files.Count; j++)
						{
							string absPath = System.IO.Path.Combine(rootPath, resources[i].files[j]);
							string subJson = System.IO.File.ReadAllText(absPath);
							ParseFile(subJson);
						}
						break;
					case ContentType.SkeletonMesh:
						resources[i] = UnityEngine.JsonUtility.FromJson<StructureReference>(json.list[i].ToString());
						break;
					case ContentType.Texture:
						resources[i] = UnityEngine.JsonUtility.FromJson<TextureReference >(json.list[i].ToString());
						break;
				}
				system.AddResource(resources[i]);
			}
		}
	}
}