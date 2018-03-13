namespace Alice.Tweedle.Unlinked.Resource
{
	[System.Serializable]
	public class JointDescription
	{
		public JointVisibilityType Visibility
		{
			get
			{
				return (JointVisibilityType)System.Enum.Parse(typeof(JointVisibilityType), visibility);
			}
		}

		public string name;
		public string parent;
		public string visibility;
	}
}