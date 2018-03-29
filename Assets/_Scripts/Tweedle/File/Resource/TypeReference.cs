namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class TypeReference : ResourceReference
	{
		public override ContentType ContentType => ContentType.Type;
	}
}