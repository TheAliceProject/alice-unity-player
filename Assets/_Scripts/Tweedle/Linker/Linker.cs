using System.Collections.Generic;

namespace Alice.Linker
{
    public class Linker
    {
        private HashSet<ProjectIdentifier> loadedFiles;
		private Dictionary<Tuple<string, ProjectType>, AssetDescription> unlinkedAssets;
		private Dictionary<string, LibraryDescription> unlinkedLibraries;
		private Dictionary<string, ProgramDescription> unlinkedPrograms;
		private Dictionary<string, TweedleLexer> unlinkedClasses;
		private Dictionary<string, ModelDescription>   unlinkedModels;
		private Dictionary<string, ResourceDescription> unlinkedResources;
        private List<AssetDescription> assets;

        public void AddClass(ProjectIdentifier identifier, TweedleLexer classLex)
        {
            loadedFiles.Add(identifier);
			unlinkedClasses.Add(identifier.id, classLex);
			//unlinkedAssets.Add(new Tuple<string, ProjectType>(identifier.id, ProjectType.Library), libAsset);
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