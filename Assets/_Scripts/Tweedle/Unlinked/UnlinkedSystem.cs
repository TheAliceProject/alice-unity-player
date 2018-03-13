using System.Collections.Generic;

namespace Alice.Tweedle.Unlinked
{
	public class UnlinkedSystem
	{
		private HashSet<ProjectIdentifier> loadedFiles;
		private HashSet<Resource.ResourceIdentifier> loadedResources;

		private Dictionary<Tuple<string, ProjectType>, AssetDescription> unlinkedAssets;
		private Dictionary<Resource.ResourceIdentifier, ResourceDescription> unlinkedResources;

		private Dictionary<string, LibraryDescription> unlinkedLibraries;
		private Dictionary<string, ProgramDescription> unlinkedPrograms;
		private Dictionary<string, ModelDescription> unlinkedModels;
		private Dictionary<string, UnlinkedClass> unlinkedClasses;
		private Dictionary<string, UnlinkedEnum> unlinkedEnums;

		public UnlinkedSystem()
		{
			loadedFiles = new HashSet<ProjectIdentifier>();
			loadedResources = new HashSet<Resource.ResourceIdentifier>();

			unlinkedAssets = new Dictionary<Tuple<string, ProjectType>, AssetDescription>();
			unlinkedResources = new Dictionary<Resource.ResourceIdentifier, ResourceDescription>();

			unlinkedLibraries = new Dictionary<string, LibraryDescription>();
			unlinkedPrograms = new Dictionary<string, ProgramDescription>();
			unlinkedModels = new Dictionary<string, ModelDescription>();
		}

		public void AddLibrary(LibraryDescription libAsset)
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

		public void AddModel(ModelDescription modelAsset)
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

		public void AddResource(ResourceDescription resourceAsset)
		{
			Resource.ResourceIdentifier identifier = new Resource.ResourceIdentifier(resourceAsset.id, resourceAsset.ContentType, resourceAsset.FormatType);
			loadedResources.Add(identifier);
			unlinkedResources.Add(identifier, resourceAsset);
		}
	}
}