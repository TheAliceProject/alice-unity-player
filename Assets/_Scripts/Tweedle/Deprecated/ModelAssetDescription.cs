namespace Alice.Linker.Deprecated
{
	[System.Serializable]
    public class ModelAssetDescription : Alice.Tweedle.Unlinked.AssetDescription
    {
        public Alice.Tweedle.Unlinked.ModelDescription Model
        {
            get { return model; }
			set { model = value; }
        }

        private Alice.Tweedle.Unlinked.ModelDescription model;

		public ModelAssetDescription(Alice.Tweedle.Unlinked.AssetDescription asset) : base(asset)
		{
		}
	}
}
