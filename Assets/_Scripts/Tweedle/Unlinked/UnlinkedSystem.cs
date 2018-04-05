using System.Collections.Generic;
using Alice.Tweedle.File;

namespace Alice.Tweedle.Unlinked
{
	public class UnlinkedSystem
	{
		private HashSet<ProjectIdentifier> loadedFiles;
		private HashSet<ResourceIdentifier> loadedResources;

		private Dictionary<Tuple<string, ProjectType>, Manifest> unlinkedAssets;
		private Dictionary<ResourceIdentifier, ResourceReference> unlinkedResources;

		private Dictionary<string, LibraryManifest> unlinkedLibraries;
		private Dictionary<string, ProgramDescription> unlinkedPrograms;
		private Dictionary<string, ModelManifest> unlinkedModels;
		private Dictionary<string, TweedleClass> classes;
		private Dictionary<string, TweedleEnum> enums;
		private Dictionary<string, TweedleType> types;

		public UnlinkedSystem()
		{
			loadedFiles = new HashSet<ProjectIdentifier>();
			loadedResources = new HashSet<ResourceIdentifier>();

			unlinkedAssets = new Dictionary<Tuple<string, ProjectType>, Manifest>();
			unlinkedResources = new Dictionary<ResourceIdentifier, ResourceReference>();

			unlinkedLibraries = new Dictionary<string, LibraryManifest>();
			unlinkedPrograms = new Dictionary<string, ProgramDescription>();
			unlinkedModels = new Dictionary<string, ModelManifest>();

			classes = new Dictionary<string, TweedleClass>();
			enums = new Dictionary<string, TweedleEnum>();
			types = new Dictionary<string, TweedleType>();
		}

		public void AddLibrary(LibraryManifest libAsset)
		{
			loadedFiles.Add(libAsset.Identifier);
			unlinkedLibraries.Add(libAsset.Identifier.id, libAsset);
			unlinkedAssets.Add(new Tuple<string, ProjectType>(libAsset.Name, ProjectType.Library), libAsset);
		}

		public void AddProgram(ProgramDescription programAsset)
		{
			loadedFiles.Add(programAsset.Identifier);
			unlinkedPrograms.Add(programAsset.Identifier.id, programAsset);
			unlinkedAssets.Add(new Tuple<string, ProjectType>(programAsset.Name, ProjectType.World), programAsset);
		}

		public void AddModel(ModelManifest modelAsset)
		{
			loadedFiles.Add(modelAsset.Identifier);
			unlinkedModels.Add(modelAsset.Identifier.id, modelAsset);
			unlinkedAssets.Add(new Tuple<string, ProjectType>(modelAsset.Name, ProjectType.Model), modelAsset);
		}

		public void AddClass(TweedleClass tweClass)
		{
			classes.Add(tweClass.Name, tweClass);
			types.Add(tweClass.Name, tweClass);

		}

		public void AddEnum(TweedleEnum tweEnum)
		{
			enums.Add(tweEnum.Name, tweEnum);
			types.Add(tweEnum.Name, tweEnum);
		}

		public void AddResource(ResourceReference resourceAsset)
		{
			ResourceIdentifier identifier = new ResourceIdentifier(resourceAsset.id, resourceAsset.ContentType, resourceAsset.FormatType);
			loadedResources.Add(identifier);
			unlinkedResources.Add(identifier, resourceAsset);
		}
	}
}