using Alice.Tweedle;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using Alice.Player.Primitives;

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
                    entity = SGEntity.Create<SGBox>(model);
                    break;
                case SPHERE:
                    entity = SGEntity.Create<SGSphere>(model);
                    break;
                default:
                    throw new SceneGraphException("No model resource found for " + resource);
            }

            UnitySceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void createSceneEntity(TValue scene) {
            var entity = SGEntity.Create<SGScene>(scene);
            UnitySceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void setEntityName(TValue thing, string name) {
            var entity = UnitySceneGraph.Current.FindEntity(thing);
            entity?.SetName(name);
        }

        [PInteropMethod]
        public static void bindTransformationProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.BindProperty(SGModel.TRANSFORMATION_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindSizeProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.BindProperty(SGModel.SIZE_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindPaintProperty(TValue owner, TValue property, TValue value) {
            UnitySceneGraph.Current.BindProperty(SGModel.PAINT_PROPERTY_NAME, owner, property, value);
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
        public static VantagePoint setVehicle(TValue vehicle, TValue rider) {
            var entity = UnitySceneGraph.Current.FindEntity(rider);
            entity.vehicle = UnitySceneGraph.Current.FindEntity(vehicle);
            
            var p = entity.cachedTransform.localPosition;
            var r = entity.cachedTransform.localRotation;
            return new VantagePoint(new Primitives.Vector3(p.x, p.y, p.z), new Primitives.Quaternion(r.x, r.y, r.z, r.w));
        }

        [PInteropMethod]
        public static TValue getVehicle(TValue rider) {
            var riderEnt = UnitySceneGraph.Current.FindEntity(rider);
            return riderEnt?.vehicle == null ? TValue.NULL : riderEnt.vehicle.owner;
        }

        [PInteropMethod]
        public static VantagePoint getVantagePoint(TValue viewer, TValue target) {
            if (ReferenceEquals(viewer.RawObject<object>(), target.RawObject<object>())) {
                return VantagePoint.IDENTITY;
            }

            var sgViewer = UnitySceneGraph.Current.FindEntity(viewer);
            var sgTarget = UnitySceneGraph.Current.FindEntity(target);

            if (sgTarget == null) {
                throw new SceneGraphException("Scene graph entity for found for target.");
            }

            var p = sgViewer.cachedTransform.position;
            var r = sgViewer.cachedTransform.rotation;

            if (sgViewer != null) {
                var tp = -sgTarget.cachedTransform.position;
                var tr = UnityEngine.Quaternion.Inverse(sgTarget.cachedTransform.rotation);

                p = tr * (p + tp);
                r = tr * r;
            }

            return new VantagePoint(new Primitives.Vector3(p.x, p.y, p.z), new Primitives.Quaternion(r.x, r.y, r.z, r.w));
        }

        [PInteropMethod]
        public static VantagePoint getCompositeTransformation(TValue thing) {
            var entity = UnitySceneGraph.Current.FindEntity(thing);
            if (entity) {
                var p = entity.cachedTransform.position;
                var r = entity.cachedTransform.rotation;
                return new VantagePoint(new Primitives.Vector3(p.x, p.y, p.z), new Primitives.Quaternion(r.x, r.y, r.z, r.w));
            }
            return VantagePoint.IDENTITY;
        }

        [PInteropMethod]
        public static VantagePoint getInverseCompositeTransformation(TValue thing) {
            var entity = UnitySceneGraph.Current.FindEntity(thing);
            if (entity) {
                var p = -entity.cachedTransform.position;
                var r = UnityEngine.Quaternion.Inverse(entity.cachedTransform.rotation);
                return new VantagePoint(new Primitives.Vector3(p.x, p.y, p.z), new Primitives.Quaternion(r.x, r.y, r.z, r.w));
            }
            return VantagePoint.IDENTITY;
        }

    }
}