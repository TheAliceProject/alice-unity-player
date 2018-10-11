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
        public static void createSceneEntity(TValue scene) {
            SGEntity entity = SGEntity.Create<SGScene>(scene, "SceneEntity");
            UnitySceneGraph.Current.AddEntity(entity);
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
        public static void setVehicle(TValue vehicle, TValue rider) {
            var entity = UnitySceneGraph.Current.FindEntity(rider);
            entity.vehicle = UnitySceneGraph.Current.FindEntity(vehicle);
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

            UnityEngine.Matrix4x4 m;
            if (sgViewer != null) {
                m = sgTarget.cachedTransform.worldToLocalMatrix;
            } else {
                var tm = sgTarget.cachedTransform.worldToLocalMatrix;
                var vm = sgViewer.cachedTransform.localToWorldMatrix;
                m = vm * tm;
            }

            return new VantagePoint(m.m00, m.m01, m.m02, m.m03,
                                    m.m10, m.m11, m.m12, m.m13,
                                    m.m20, m.m21, m.m22, m.m23,
                                    m.m30, m.m31, m.m32, m.m33);
        }

        [PInteropMethod]
        public static VantagePoint getCompositeTransformation(TValue thing) {
            var entity = UnitySceneGraph.Current.FindEntity(thing);
            if (entity) {
                var m = entity.cachedTransform.localToWorldMatrix;
                return new VantagePoint(m.m00, m.m01, m.m02, m.m03,
                                        m.m10, m.m11, m.m12, m.m13,
                                        m.m20, m.m21, m.m22, m.m23,
                                        m.m30, m.m31, m.m32, m.m33);
            }
            return VantagePoint.IDENTITY;
        }

        [PInteropMethod]
        public static VantagePoint getInverseCompositeTransformation(TValue thing) {
            var entity = UnitySceneGraph.Current.FindEntity(thing);
            if (entity) {
                var m = entity.cachedTransform.localToWorldMatrix;
                return new VantagePoint(m.m00, m.m01, m.m02, m.m03,
                                        m.m10, m.m11, m.m12, m.m13,
                                        m.m20, m.m21, m.m22, m.m23,
                                        m.m30, m.m31, m.m32, m.m33);
            }
            return VantagePoint.IDENTITY;
        }
    }
}