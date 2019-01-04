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

        #region Entity Instantiation
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

            SceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void createSceneEntity(TValue scene) {
            var entity = SGEntity.Create<SGScene>(scene);
            SceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void createCameraEntity(TValue camera) {
            var entity = SGEntity.Create<SGCamera>(camera);
            SceneGraph.Current.AddEntity(entity);
        }
        #endregion // Entity Instantiation

        #region Property Binding
        [PInteropMethod]
        public static void bindTransformationProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGModel.TRANSFORMATION_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindSizeProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGModel.SIZE_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindPaintProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGModel.PAINT_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindRadiusProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGSphere.RADIUS_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindOpacityProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGModel.OPACITY_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindFogDensityProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGScene.FOG_DENSITY_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindAtmosphereColorProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGScene.ATMOSPHERE_COLOR_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindGlobalBrightnessProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGScene.GLOBAL_BRIGHTNESS_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindAmbientLightColorProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGScene.AMBIENT_LIGHT_COLOR_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindFromAboveLightColorProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGScene.ABOVE_LIGHT_COLOR_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindFromBelowLightColorProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGScene.BELOW_LIGHT_COLOR_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void updateProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.UpdateProperty(owner, property, value);
        }
        #endregion // Property Binding

        #region Other

        [PInteropMethod]
        public static void setEntityName(TValue thing, string name) {
            var entity = SceneGraph.Current.FindEntity(thing);
            entity.SetName(name);
        }

        [PInteropMethod]
        public static VantagePoint setVehicle(TValue vehicle, TValue rider) {
            var entity = SceneGraph.Current.FindEntity(rider);
            entity.vehicle = SceneGraph.Current.FindEntity(vehicle);
            
            var p = entity.cachedTransform.localPosition;
            var r = entity.cachedTransform.localRotation;
            return VantagePoint.FromUnity(p, r);
        }

        [PInteropMethod]
        public static TValue getVehicle(TValue rider) {
            var riderEnt = SceneGraph.Current.FindEntity(rider);
            return riderEnt?.vehicle == null ? TValue.NULL : riderEnt.vehicle.owner;
        }

        [PInteropMethod]
        public static void addSceneActivationListener(TValue scene, PAction listener) {
            var entity = SceneGraph.Current.FindEntity<SGScene>(scene);
            entity.AddActivationListener(listener);
        }

        [PInteropMethod]
        public static void activateScene(TValue scene) {
            var entity = SceneGraph.Current.FindEntity<SGScene>(scene);
            entity.Activate();
        }
        #endregion // Other

        #region Transformations
        [PInteropMethod]
        public static VantagePoint getVantagePoint(TValue viewer, TValue target) {
            if (ReferenceEquals(viewer.RawObject<object>(), target.RawObject<object>())) {
                return VantagePoint.IDENTITY;
            }

            return getInverseAbsoluteTransformation(viewer).multiply(getAbsoluteTransformation(target));
        }

        [PInteropMethod]
        public static VantagePoint getAbsoluteTransformation(TValue thing) {
            var entity = SceneGraph.Current.FindEntity(thing);
            if (entity) {
                var p = entity.cachedTransform.position;
                var r = entity.cachedTransform.rotation;
                return VantagePoint.FromUnity(p, r);
            }
            return VantagePoint.IDENTITY;
        }

        [PInteropMethod]
        public static VantagePoint getInverseAbsoluteTransformation(TValue thing) {
            var entity = SceneGraph.Current.FindEntity(thing);
            if (entity) {
                var r = UnityEngine.Quaternion.Inverse(entity.cachedTransform.rotation);
                var p = r*-entity.cachedTransform.position;
                return VantagePoint.FromUnity(p, r);
            }
            return VantagePoint.IDENTITY;
        }

        [PInteropMethod]
        public static Position getAbsolutePosition(TValue thing) {
            var entity = SceneGraph.Current.FindEntity(thing);
            if (entity) {
                return Position.FromUnity(entity.cachedTransform.position);
            } else {
                throw new SceneGraphException("No scene graph entity exists for tweedle object.");
            }
        }

        [PInteropMethod]
        public static AxisAlignedBox getLocalBoundingBox(TValue model, bool dynamic = false) {
            var entity = SceneGraph.Current.FindEntity(model);
            if (entity && entity is SGModel) {
                var sgModel = (SGModel)entity;
                return sgModel.GetBounds(dynamic);
            }
            return AxisAlignedBox.EMPTY;
        }
        #endregion // Transformations
    }
}
