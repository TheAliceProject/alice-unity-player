using System.Collections;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ImageReference : ResourceReference
    {
        public override ContentType ContentType => ContentType.Image;

        public string uuid;
        public float height;
        public float width;

        public override IEnumerator LoadContent(JsonParser parser, string workingDirectory) {
            parser.LoadTexture(this, workingDirectory);
            yield return null;
        }
    }
}