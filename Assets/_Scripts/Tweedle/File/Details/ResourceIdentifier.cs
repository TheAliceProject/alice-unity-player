namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ResourceIdentifier
    {
        public string id;
        public ContentType contentType;
        public string format;

        public ResourceIdentifier(string id, ContentType contentType, string format)
        {
            this.id = id;
            this.contentType = contentType;
            this.format = format;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ResourceIdentifier)) return false;
            return Equals((ResourceIdentifier)obj);
        }

        public bool Equals(ResourceIdentifier other)
        {
            return id.Equals(id) && contentType.Equals(other.contentType) && format.Equals(other.format);
        }

        public override int GetHashCode()
        {
            return new { A = id, B = contentType, C = format }.GetHashCode();
        }
    }
}