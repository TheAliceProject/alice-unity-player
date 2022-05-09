using System;

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

        public int CompareCompatibility(ProjectIdentifier other) {
            if (!name.Equals(other.name) || !type.Equals(other.type))
                return -1;
            var majorVersion = GetMajorVersion();
            var otherMajorVersion = other.GetMajorVersion();
            // Pre-release. Not stable yet. Must match version exactly.
            if (majorVersion == 0 || otherMajorVersion == 0)
                return StringComparer.Ordinal.Compare(version, other.version);
            // Once stable (1.0), major version change indicates breaking change
            return otherMajorVersion - majorVersion;
        }

        private int GetMajorVersion() {
            return int.Parse(version.Substring(0, version.IndexOf('.')));
        }

        // TODO Expand identifier to follow semver and include patch, pre-release, and build
        private int GetMinorVersion() {
            return int.Parse(version.Substring(version.IndexOf('.') + 1));
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