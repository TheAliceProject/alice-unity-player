namespace Alice.Linker
{
	[System.Serializable]
    public class ModelAssetDescription : AssetDescription
    {
        public ModelDescription Description
        {
            get { return description; }
			set { description = value; }
        }

        public string Name
        {
            get { return description.name; }
        }

        private ModelDescription description;

		public ModelAssetDescription(AssetDescription asset) : base(asset)
		{
		}
	}
}