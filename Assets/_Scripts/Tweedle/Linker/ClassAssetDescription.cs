namespace Alice.Linker
{
    public class ClassAssetDescription : AssetDescription
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

		public ClassAssetDescription(AssetDescription toCopy) : base(toCopy) 
		{
		}
	}
}