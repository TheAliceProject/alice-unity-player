using System.Collections.Generic;
using Alice.Tweedle.File;

namespace Alice.Tweedle.Parsed
{
	public class TweedleSystem
	{
		public HashSet<ProjectIdentifier> LoadedFiles { get; private set; }
		public HashSet<ResourceIdentifier> LoadedResources { get; private set; }

		public Dictionary<Tuple<string, ProjectType>, Manifest> UnlinkedAssets { get; private set; }
		public Dictionary<ResourceIdentifier, ResourceReference> UnlinkedResources { get; private set; }

		public Dictionary<string, LibraryManifest> UnlinkedLibraries { get; private set; }
		public Dictionary<string, ProgramDescription> UnlinkedPrograms { get; private set; }
		public Dictionary<string, ModelManifest> UnlinkedModels { get; private set; }
		public Dictionary<string, TweedleClass> Classes { get; private set; }
		public Dictionary<string, TweedleEnum> Enums { get; private set; }
		public Dictionary<string, TweedleType> Types { get; private set; }

		public TweedleSystem()
		{
			LoadedFiles = new HashSet<ProjectIdentifier>();
			LoadedResources = new HashSet<ResourceIdentifier>();

			UnlinkedAssets = new Dictionary<Tuple<string, ProjectType>, Manifest>();
			UnlinkedResources = new Dictionary<ResourceIdentifier, ResourceReference>();

			UnlinkedLibraries = new Dictionary<string, LibraryManifest>();
			UnlinkedPrograms = new Dictionary<string, ProgramDescription>();
			UnlinkedModels = new Dictionary<string, ModelManifest>();

			Classes = new Dictionary<string, TweedleClass>();
			Enums = new Dictionary<string, TweedleEnum>();
			Types = new Dictionary<string, TweedleType>();
		}

		public void AddLibrary(LibraryManifest libAsset)
		{
			LoadedFiles.Add(libAsset.Identifier);
			UnlinkedLibraries.Add(libAsset.Identifier.id, libAsset);
			UnlinkedAssets.Add(new Tuple<string, ProjectType>(libAsset.Name, ProjectType.Library), libAsset);
		}

		public void AddProgram(ProgramDescription programAsset)
		{
			LoadedFiles.Add(programAsset.Identifier);
			UnlinkedPrograms.Add(programAsset.Identifier.id, programAsset);
			UnlinkedAssets.Add(new Tuple<string, ProjectType>(programAsset.Name, ProjectType.World), programAsset);
		}

		public void AddModel(ModelManifest modelAsset)
		{
			LoadedFiles.Add(modelAsset.Identifier);
			UnlinkedModels.Add(modelAsset.Identifier.id, modelAsset);
			UnlinkedAssets.Add(new Tuple<string, ProjectType>(modelAsset.Name, ProjectType.Model), modelAsset);
		}

		public void AddClass(TweedleClass tweClass)
		{
			Classes.Add(tweClass.Name, tweClass);
			Types.Add(tweClass.Name, tweClass);

		}

		public void AddEnum(TweedleEnum tweEnum)
		{
			Enums.Add(tweEnum.Name, tweEnum);
			Types.Add(tweEnum.Name, tweEnum);
		}

		public void AddResource(ResourceReference resourceAsset)
		{
			ResourceIdentifier identifier = new ResourceIdentifier(resourceAsset.id, resourceAsset.ContentType, resourceAsset.FormatType);
			LoadedResources.Add(identifier);
			UnlinkedResources.Add(identifier, resourceAsset);
		}
	}
}