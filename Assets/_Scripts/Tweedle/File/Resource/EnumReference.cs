namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class EnumReference : ResourceReference
	{
		public override ContentType ContentType => ContentType.Enum;
	}
}