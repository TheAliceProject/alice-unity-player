using System.Collections;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ClassReference : ResourceReference
    {
        public override ContentType ContentType => ContentType.Class;

        public override IEnumerator LoadContent(JsonParser parser, string workingDirectory) {
            parser.ParseTweedleTypeResource(this, workingDirectory);
            yield return null;
        }
    }
}