namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class ImageReference : ResourceReference
	{
		public override ContentType ContentType => ContentType.Image;

		public string uuid;
		public float height;
		public float width;
	}
}