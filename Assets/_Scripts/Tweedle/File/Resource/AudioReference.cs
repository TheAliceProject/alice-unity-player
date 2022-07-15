using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class AudioReference : ResourceReference
    {
        public string uuid;
        public float duration;

        public override ContentType ContentType => ContentType.Audio;

        public override void LoadContent(JsonParser parser, string workingDirectory) {
            parser.LoadAudio(this, workingDirectory);
        }
    }
}