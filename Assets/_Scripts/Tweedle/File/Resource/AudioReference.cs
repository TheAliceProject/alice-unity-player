namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class AudioReference : ResourceReference
	{
		public System.Guid uuid; // TODO
		public float duration;

		public override ContentType ContentType => ContentType.Audio;
	}
}