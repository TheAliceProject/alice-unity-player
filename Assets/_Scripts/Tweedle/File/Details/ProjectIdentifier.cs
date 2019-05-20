namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ProjectIdentifier
    {
        public string name;
        public string version;
        public string type;

        public ProjectType Type
        {
            get
            {
                return (ProjectType)System.Enum.Parse(typeof(ProjectType), type);
            }
        }

        public ProjectIdentifier(string name, string version, string type)
        {
            this.name = name;
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
            return name.Equals(other.name) && version.Equals(other.version) && type.Equals(other.type);
        }

        public override int GetHashCode()
        {
            return new { A = name, B = version, C = type }.GetHashCode();
        }
    }
}