namespace Alice.Json
{
	public class Core
	{
        // read in tweedle file
        private void Parse(Linker.Linker program)
        {
            Linker.ProjectType t = Linker.ProjectType.Class; // read in json
            switch (t)
            {
                case Linker.ProjectType.Class:
                    Linker.ClassAssetDescription classAsset = null;
                    // Parse JSON into a
                    program.AddClass(classAsset);
                    break;
                case Linker.ProjectType.Library:
                    Linker.LibraryDescription libAsset = null;
                    program.AddLibrary(libAsset);
                    break;
                case Linker.ProjectType.World:
                    Linker.ProgramDescription worldAsset = null;
                    program.AddProgram(worldAsset);
                    break;
                case Linker.ProjectType.Model:
                    Linker.ModelAssetDescription modelAsset = null;
                    program.AddModel(modelAsset);
                    break;
            }
        }
	}
}