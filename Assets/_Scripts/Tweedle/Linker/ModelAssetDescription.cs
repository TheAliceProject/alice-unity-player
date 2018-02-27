namespace Alice.Linker
{
    public class ModelAssetDescription : AssetDescription
    {
        public ModelDescription Description
        {
            get { return description; }
        }

        public string Name
        {
            get { return description.Name; }
        }

        private ModelDescription description;
    }
}