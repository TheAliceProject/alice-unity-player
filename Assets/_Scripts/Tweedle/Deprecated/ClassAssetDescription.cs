namespace Alice.Tweedle.Unlinked.Deprecated
{
    public class ClassAssetDescription : Alice.Tweedle.Unlinked.AssetDescription
    {
        public ClassDescription Description
        {
            get { return description; }
        }

        public string Name
        {
            get { return description.Name; }
        }

        private ClassDescription description;

		public ClassAssetDescription(Alice.Tweedle.Unlinked.AssetDescription toCopy) : base(toCopy) 
		{
		}
	}
}