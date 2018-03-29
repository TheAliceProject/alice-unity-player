using System.Collections.Generic;

namespace Alice.Tweedle.File
{
	[System.Serializable]
	public class TextureReference : ResourceReference
	{
		public override ContentType ContentType => ContentType.Texture;
	}
}