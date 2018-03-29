namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class ImageReference : ResourceReference
	{
		public override ContentType ContentType => ContentType.Image;

		public System.Guid uuid; // TODO
		public float height;
		public float width;
	}
}