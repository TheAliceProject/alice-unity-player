using System.Collections;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ModelReference : ResourceReference
    {
        public override ContentType ContentType => ContentType.Model;

        public override IEnumerator LoadContent(JsonParser parser, string workingDirectory) {
            yield return parser.LoadModel(this, workingDirectory);
        }
    }
}