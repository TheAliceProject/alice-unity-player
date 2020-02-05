namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class AudioReference : ResourceReference
    {
        public string uuid;
        public float duration;

        public override ContentType ContentType => ContentType.Audio;
    }
}