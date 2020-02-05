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

        public bool EqualsVersionMatch(ProjectIdentifier other)
        {
            return name.Equals(other.name) && VersionEqualOrGreater(version, other.version) && type.Equals(other.type);
        }

        public bool Equals(ProjectIdentifier other)
        {
            return name.Equals(other.name) && version.Equals(other.version) && type.Equals(other.type);
        }

        public bool VersionEqualOrGreater(string libraryVersion, string testVersion)
        {
            int libVer = int.Parse(libraryVersion.Substring(libraryVersion.IndexOf('.') + 1));
            int testVer = int.Parse(testVersion.Substring(testVersion.IndexOf('.') + 1));

            return libVer >= testVer;
        }

        public override int GetHashCode()
        {
            return new { A = name, B = version, C = type }.GetHashCode();
        }
    }
}