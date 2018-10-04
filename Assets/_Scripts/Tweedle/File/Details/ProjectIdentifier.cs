namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ProjectIdentifier
    {
        public string id;
        public string version;
        public string type;

        public ProjectType Type
        {
            get
            {
                return (ProjectType)System.Enum.Parse(typeof(ProjectType), type);
            }
        }

        public ProjectIdentifier(string id, string version, string type)
        {
            this.id = id;
            this.version = version;
            this.type = type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ProjectIdentifier)) return false;
            return Equals((ProjectIdentifier)obj);
        }

        public bool Equals(ProjectIdentifier other)
        {
            return id.Equals(id) && version.Equals(other.version) && type.Equals(other.type);
        }

        public override int GetHashCode()
        {
            return new { A = id, B = version, C = type }.GetHashCode();
        }
    }
}