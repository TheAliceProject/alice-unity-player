using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ModelReference : ResourceReference
    {
        public override ContentType ContentType => ContentType.Model;

        public override void LoadContent(JsonParser parser, string workingDirectory) {
            parser.LoadModel(this, workingDirectory);
        }
    }
}