using Alice.Tweedle;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;

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

            SGEntity entity = null;
            switch (resource) {
                case BOX:
                    entity = SGEntity.Create<SGBox>(model, "BoxEntity");
                    break;
                case SPHERE:
                    entity = SGEntity.Create<SGSphere>(model, "SphereEntity");
                    break;
                default:
                    throw new SceneGraphException("No model resource found for " + resource);
            }

            UnitySceneGraph.Current.AddEntity(entity);
            return entity;
        }
    }
}