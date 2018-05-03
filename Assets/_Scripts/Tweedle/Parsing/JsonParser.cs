using Alice.Tweedle.File;
using System.Collections.Generic;

namespace Alice.Tweedle.Parsed
{
	public class JsonParser
	{
		private string rootPath = "";
		private TweedleSystem system;
		private TweedleParser tweedleParser;

		public TweedleSystem StoredSystem
		{
			get { return system; }
		}

		public JsonParser(string root)
		{
			this.system = new TweedleSystem();
			this.rootPath = root;
			this.tweedleParser = new TweedleParser();
		}

		public JsonParser(TweedleSystem system, string root)
		{
			this.system = system;
			this.rootPath = root;
			this.tweedleParser = new TweedleParser();
		}

		public void ParseJson(string json)
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
					break;
                case ProjectType.World:
                    ProgramDescription worldAsset =  new ProgramDescription(asset);
					system.AddProgram(worldAsset);
					break;
                case ProjectType.Model:
					ModelManifest modelAsset = UnityEngine.JsonUtility.FromJson<ModelManifest>(json);
					system.AddModel(modelAsset);
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
				for (int j = 0; j < resources[i].files.Count; j++)
				{
					string absPath = System.IO.Path.Combine(rootPath, resources[i].files[j]);
					if (!System.IO.File.Exists(absPath))
					{
						UnityEngine.Debug.LogWarning("Missing class file: " + absPath);
					}
				}
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
							TweedleClass tweClass = (TweedleClass)tweedleParser.ParseType(System.IO.File.ReadAllText(absPath));
							system.AddClass(tweClass);
						}
						break;
					case ContentType.Enum:
						resources[i] = UnityEngine.JsonUtility.FromJson<EnumReference>(json.list[i].ToString());
						for (int j = 0; j < resources[i].files.Count; j++)
						{
							string absPath = System.IO.Path.Combine(rootPath, resources[i].files[j]);
							TweedleEnum tweEnum = (TweedleEnum)tweedleParser.ParseType(System.IO.File.ReadAllText(absPath));
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
							ParseJson(subJson);
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