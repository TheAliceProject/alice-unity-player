using System.Collections.Generic;

namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class ModelReference : ResourceReference
    {
        public override ContentType ContentType => ContentType.Model;
    }
}