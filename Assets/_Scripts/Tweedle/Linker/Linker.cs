using System.Collections.Generic;

namespace Alice.Linker
{
    public class Linker
    {
        private HashSet<ProjectIdentifier> loadedFiles;
		//private Dictionary<string, Tuple<Deprecated.ClassDescription, Tweedle.TweedleClass>> classes;
		//private Dictionary<string, ClassDescription> unlinkedClasses;
		private Dictionary<string, AssetDescription> unlinkedAssets;
		private Dictionary<string, ResourceDescription> unlinkedResources;
        private List<AssetDescription> assets;

        /*public void AddClass(Deprecated.ClassAssetDescription classAsset)
        {
            loadedFiles.Add(classAsset.package.identifier);
            assets.Add(classAsset);
            classes.Add(classAsset.Name, new Tuple<Deprecated.ClassDescription, Tweedle.TweedleClass>(classAsset.Description, null));
            //unlinkedClasses.Add(asset.Name, asset.Description);
        }*/

        public void AddLibrary(LibraryDescription libAsset)
        {
			unlinkedAssets.Add(libAsset.Name, libAsset);
        }

        public void AddProgram(ProgramDescription programAsset)
        {
			unlinkedAssets.Add(programAsset.Name, programAsset);
		}

        public void AddModel(ModelDescription modelAsset)
        {
			unlinkedAssets.Add(modelAsset.Name, modelAsset);
		}

		public Tweedle.TweedleProgram Link()
        {
            // (paritally) order classes
            // link the class heirarchy
            // then go through each class method
            // if depends on child class throw error (gracefully)
            return null;
        }

        /*public Deprecated.ClassDescription NewUnlinkedClass(string name)
        {
            if (classes.ContainsKey(name))
            {

            }
			Deprecated.ClassDescription classNew = new Deprecated.ClassDescription(name);
            classes.Add(name, new Tuple<Deprecated.ClassDescription, Tweedle.TweedleClass>(classNew, null));
            return classNew;
        }*/

        /*public TweedleType GetLinkedTypeNamed(string name)
        {

        }*/
    }
}