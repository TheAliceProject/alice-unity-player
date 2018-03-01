using System.Collections.Generic;

namespace Alice.Linker
{
    public class Linker
    {
        private HashSet<ProjectIdentifier> loadedFiles;
        private Dictionary<string, Tuple<ClassDescription, Tweedle.TweedleClass>> classes;
        //private Dictionary<string, ClassDescription> unlinkedClasses;
        private Dictionary<string, ResourceDescription> unlinkedResources;
        private List<AssetDescription> assets;

        public void AddClass(ClassAssetDescription classAsset)
        {
            loadedFiles.Add(classAsset.identifier);
            assets.Add(classAsset);
            classes.Add(classAsset.Name, new Tuple<ClassDescription, Tweedle.TweedleClass>(classAsset.Description, null));
            //unlinkedClasses.Add(asset.Name, asset.Description);
        }

        public void AddLibrary(LibraryDescription libAsset)
        {
        }

        public void AddProgram(ProgramDescription programAsset)
        {
        }

        public void AddModel(ModelAssetDescription modelAsset)
        {
        }

        public Tweedle.TweedleProgram Link()
        {
            // (paritally) order classes
            // link the class heirarchy
            // then go through each class method
            // if depends on child class throw error (gracefully)
            return null;
        }

        public ClassDescription NewUnlinkedClass(string name)
        {
            if (classes.ContainsKey(name))
            {

            }
            ClassDescription classNew = new ClassDescription(name);
            classes.Add(name, new Tuple<ClassDescription, Tweedle.TweedleClass>(classNew, null));
            return classNew;
        }
/*
        public TweedleType GetLinkedTypeNamed(string name)
        {

        }
*/
    }
}