using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File {
    [System.Serializable]
    public class EnumReference : ResourceReference
    {
        public override ContentType ContentType => ContentType.Enum;

        public override void LoadContent(JsonParser parser, string workingDirectory) {
            parser.ParseTweedleTypeResource(this, workingDirectory);
        }
    }
}