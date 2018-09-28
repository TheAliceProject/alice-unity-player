using Alice.Tweedle;
using Alice.Tweedle.Interop;

namespace Alice.Player.Modules {
    [PInteropType("SceneGraph")]
    static public class SceneGraphModule
    {

        [PInteropField]
        public const string BOX = "Internal/Box";
        [PInteropField]
        public const string CONE = "Internal/Cone";
        [PInteropField]
        public const string CYLINDER = "Internal/Cylinder";
        [PInteropField]
        public const string DISC = "Internal/Disc";
        [PInteropField]
        public const string SPHERE = "Internal/Sphere";
        [PInteropField]
        public const string TORUS = "Internal/Torus";

        [PInteropMethod]
        public static SGEntity createEntity(TValue model, string resource) {
            switch (resource) {
                case BOX:
                    return SGEntity.Create<SGBox>(model, "BoxEntity");
                case SPHERE:
                    return SGEntity.Create<SGSphere>(model, "SphereEntity");
                default:
                    throw new SceneGraphException("No model resource found for " + resource);
            }
        }
    }
}