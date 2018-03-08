using System.Collections.Generic;

namespace Alice.Linker
{
    public class Linker
    {
        private HashSet<ProjectIdentifier> loadedFiles;
		private HashSet<Resource.ResourceIdentifier> loadedResources;

		private Dictionary<Tuple<string, ProjectType>, AssetDescription> unlinkedAssets;
		private Dictionary<Resource.ResourceIdentifier, ResourceDescription> unlinkedResources;

		private Dictionary<string, LibraryDescription> unlinkedLibraries;
		private Dictionary<string, ProgramDescription> unlinkedPrograms;
		private Dictionary<string, ModelDescription>   unlinkedModels;
		private Dictionary<string, TweedleParser> unlinkedClasses;

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
		public void AddClass(TweedleParser classParsed)
		{
			unlinkedClasses.Add(classParsed.classType().ToString(), classParsed);
		}

		public void AddResource(ResourceDescription resourceAsset)
		{
			Resource.ResourceIdentifier identifier = new Resource.ResourceIdentifier(resourceAsset.id, resourceAsset.ContentType, resourceAsset.FormatType);
			loadedResources.Add(identifier);
			unlinkedResources.Add(identifier, resourceAsset);
		}

		public Tweedle.TweedleProgram Link()
        {
            // (paritally) order classes
            // link the class heirarchy
            // then go through each class method
            // if depends on child class throw error (gracefully)
            return null;
        }
    }
}