using System.Collections;
using System.Collections.Generic;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ResourceReference
    {
        public virtual ContentType ContentType
        {
            get
            {
                string type_regex = System.Text.RegularExpressions.Regex.Replace(type, @"([.,-][a-z]|\b[a-z])", m => m.Value.ToUpper());
                type_regex = type_regex.Replace("-", string.Empty);
                try
                {
                    return (ContentType)System.Enum.Parse(typeof(ContentType), type_regex);
                }
                catch (System.Exception)
                {
                    return ContentType.NULL;
                }
            }
        }

        public string FormatType
        {
            get { return format; }
        }


        public string name;
        public string type;
        public string format;
        public string file;

        // No additional loading by default. Subclasses may specialize.
        public virtual IEnumerator LoadContent(JsonParser parser, string workingDirectory) {
            yield return null;
        }
    }
}