using Alice.Utils;
namespace Alice.Tweedle.File
{
    [System.Serializable]
    public class StructureReference : ResourceReference
    {
        public override ContentType ContentType => ContentType.SkeletonMesh;

        public BoundingBox boundingBox;
    }
}