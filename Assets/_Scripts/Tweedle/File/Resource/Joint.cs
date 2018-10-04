namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class Joint
    {
        public VisibilityType Visibility
        {
            get
            {
                return (VisibilityType)System.Enum.Parse(typeof(VisibilityType), visibility);
            }
        }

        public string name;
        public string parent;
        public string visibility;
    }
}