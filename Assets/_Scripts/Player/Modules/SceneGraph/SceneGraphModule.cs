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
        public static void createEntity(TValue model, string resource) {

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
        }

        [PInteropMethod]
        public static void bindPaintProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.BindProperty(SGModel.PAINT_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindSizeProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.BindProperty(SGModel.SIZE_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindRadiusProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.BindProperty(SGSphere.RADIUS_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindOpacityProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.BindProperty(SGModel.OPACITY_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void updateProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.UpdateProperty(owner, property, value);
        }

        [PInteropMethod]
        public static void setVehicle(TValue vehicle, TValue rider) {
            var entity = UnitySceneGraph.Current.FindEntity(rider);
            entity.vehicle = UnitySceneGraph.Current.FindEntity(vehicle);
        }

        [PInteropMethod]
        public static TValue getVehicle(TValue rider) {
            var riderEnt = UnitySceneGraph.Current.FindEntity(rider);
            return riderEnt?.vehicle == null ? TValue.NULL : riderEnt.vehicle.owner;
        }
    }
}