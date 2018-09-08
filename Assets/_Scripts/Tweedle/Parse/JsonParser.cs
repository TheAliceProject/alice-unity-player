using Alice.Tweedle.File;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;

namespace Alice.Tweedle.Parse
{
	public class JsonParser
	{
		private TweedleSystem system;
		private TweedleParser tweedleParser;
		private ZipFile zipFile;

		public TweedleSystem StoredSystem
		{
			get { return system; }
		}

		public JsonParser(TweedleSystem system, ZipFile zipFile)
		{
			this.system = system;
			this.zipFile = zipFile;
			tweedleParser = new TweedleParser();
		}

		internal void Parse()
		{
			ParseJson(ReadEntry("manifest.json"));
		}

		public void ParseJson(string manifestJson)
		{
			Manifest asset = UnityEngine.JsonUtility.FromJson<Manifest>(manifestJson);
			JSONObject jsonObj = new JSONObject(manifestJson);
			ParseResourceDetails(
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
					ProgramDescription worldAsset = new ProgramDescription(asset);
					system.AddProgram(worldAsset);
					break;
				case ProjectType.Model:
					ModelManifest modelAsset = UnityEngine.JsonUtility.FromJson<ModelManifest>(manifestJson);
					system.AddModel(modelAsset);
					break;
			}
		}

		string ReadEntry(string location)
		{
			ZipEntry entry = zipFile.GetEntry(location);
			if (entry == null)
			{
				UnityEngine.Debug.Log("Did not find entry for: " + location);
			}
			return ReadEntry(entry);
		}

		string ReadEntry(ZipEntry entry)
		{
			Stream entryStream = zipFile.GetInputStream(entry);
			return (new StreamReader(entryStream)).ReadToEnd();
		}

		private void ParseResourceDetails(
			List<ResourceReference> resources,
			JSONObject json)
		{
			if (json == null || json.type != JSONObject.Type.ARRAY)
			{
				return;
			}
			for (int i = 0; i < resources.Count; i++)
			{
				ResourceReference strictResource = ReadResource(resources[i], json.list[i].ToString());
				resources[i] = strictResource;
				system.AddResource(strictResource);
			}
		}

		private ResourceReference ReadResource(ResourceReference resourceRef, string refJson)
		{
			switch (resourceRef.ContentType)
			{
				case ContentType.Audio:
					return UnityEngine.JsonUtility.FromJson<AudioReference>(refJson);
				case ContentType.Class:
					for (int j = 0; j < resourceRef.files.Count; j++)
					{
						string tweedleCode = ReadEntry(resourceRef.files[j]);
						TType tweClass = (TType)tweedleParser.ParseType(tweedleCode);
						system.AddClass(tweClass);
					}
					return UnityEngine.JsonUtility.FromJson<ClassReference>(refJson);
				case ContentType.Enum:
					for (int j = 0; j < resourceRef.files.Count; j++)
					{
						// string tweedleCode = ReadEntry(resourceRef.files[j]);
						// TweedleEnum tweedleEnum = (TweedleEnum)tweedleParser.ParseType(tweedleCode);
						// system.AddEnum(tweedleEnum);
					}
					return UnityEngine.JsonUtility.FromJson<EnumReference>(refJson);
				case ContentType.Image:
					return UnityEngine.JsonUtility.FromJson<ImageReference>(refJson);
				case ContentType.Model:
					for (int j = 0; j < resourceRef.files.Count; j++)
					{
						string subJson = System.IO.File.ReadAllText(resourceRef.files[j]);
						ParseJson(subJson);
					}
					return UnityEngine.JsonUtility.FromJson<ModelReference>(refJson);
				case ContentType.SkeletonMesh:
					return UnityEngine.JsonUtility.FromJson<StructureReference>(refJson);
				case ContentType.Texture:
					return UnityEngine.JsonUtility.FromJson<TextureReference>(refJson);
			}
			return null;
		}
	}
}
