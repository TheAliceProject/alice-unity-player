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
		private Dictionary<string, UnlinkedClass> unlinkedClasses;
		private Dictionary<string, UnlinkedEnum> unlinkedEnums;

		public UnlinkedSystem()
		{
			loadedFiles = new HashSet<ProjectIdentifier>();
			loadedResources = new HashSet<ResourceIdentifier>();

			unlinkedAssets = new Dictionary<Tuple<string, ProjectType>, Manifest>();
			unlinkedResources = new Dictionary<ResourceIdentifier, ResourceReference>();

			unlinkedLibraries = new Dictionary<string, LibraryManifest>();
			unlinkedPrograms = new Dictionary<string, ProgramDescription>();
			unlinkedModels = new Dictionary<string, ModelManifest>();
		}

		public void AddLibrary(LibraryManifest libAsset)
		{
			loadedFiles.Add(libAsset.package.identifier);
			unlinkedLibraries.Add(libAsset.Id, libAsset);
			unlinkedAssets.Add(new Tuple<string, ProjectType>(libAsset.Name, ProjectType.Library), libAsset);
		}

		public void AddProgram(ProgramDescription programAsset)
		{
			loadedFiles.Add(programAsset.package.identifier);
			unlinkedPrograms.Add(programAsset.Id, programAsset);
			unlinkedAssets.Add(new Tuple<string, ProjectType>(programAsset.Name, ProjectType.World), programAsset);
		}

		public void AddModel(ModelManifest modelAsset)
		{
			loadedFiles.Add(modelAsset.package.identifier);
			unlinkedModels.Add(modelAsset.Id, modelAsset);
			unlinkedAssets.Add(new Tuple<string, ProjectType>(modelAsset.Name, ProjectType.Model), modelAsset);
		}

		public void AddClass(UnlinkedClass unlinkedClass)
		{
			unlinkedClasses.Add(unlinkedClass.Name, unlinkedClass);
		}

		public void AddEnum(UnlinkedEnum unlinkedEnum)
		{
			unlinkedEnums.Add(unlinkedEnum.Name, unlinkedEnum);
		}

		public void AddResource(ResourceReference resourceAsset)
		{
			ResourceIdentifier identifier = new ResourceIdentifier(resourceAsset.id, resourceAsset.ContentType, resourceAsset.FormatType);
			loadedResources.Add(identifier);
			unlinkedResources.Add(identifier, resourceAsset);
		}
	}
}