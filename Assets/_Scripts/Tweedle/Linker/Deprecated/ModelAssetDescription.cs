namespace Alice.Linker.Deprecated
{
	[System.Serializable]
    public class ModelAssetDescription : AssetDescription
    {
        public ModelDescription Model
        {
            get { return model; }
			set { model = value; }
        }

        private ModelDescription model;

		public ModelAssetDescription(AssetDescription asset) : base(asset)
		{
		}
	}
}
