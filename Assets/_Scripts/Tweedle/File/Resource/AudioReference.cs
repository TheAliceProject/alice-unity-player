using System.Collections;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class AudioReference : ResourceReference
    {
        public string uuid;
        public float duration;

        public override ContentType ContentType => ContentType.Audio;

        public override IEnumerator LoadContent(JsonParser parser, string workingDirectory) {
            yield return parser.LoadAudio(this, workingDirectory);
        }
    }
}