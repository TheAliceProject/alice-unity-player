using Alice.Tweedle;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using Alice.Player.Primitives;
using UnityEngine;
using System.Collections;
using BeauRoutine;
using FlyingText3D;

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
        public const string BILLBOARD = "Internal/Billboard";
        [PInteropField]
        public const string SPHERE = "Internal/Sphere";
        [PInteropField]
        public const string TORUS = "Internal/Torus";
        [PInteropField]
        public const string AXES = "Internal/Axes";
        [PInteropField]
        public const string TEXTMODEL = "Internal/TextModel";

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
                case CYLINDER:
                    entity = SGEntity.Create<SGCylinder>(model);
                    break;
                case CONE:
                    entity = SGEntity.Create<SGCone>(model);
                    break;
                case TORUS:
                    entity = SGEntity.Create<SGTorus>(model);
                    break;
                case DISC:
                    entity = SGEntity.Create<SGDisc>(model);
                    break;
                case BILLBOARD:
                    entity = SGEntity.Create<SGBillboard>(model);
                    break;
                case AXES:
                    entity = SGEntity.Create<SGAxes>(model);
                    break;
                case TEXTMODEL:
                    var flyingText = FlyingText.instance;
                    // Instantiate the flying text object if not already instantiated. Might see a "Don't Destroy on Load" wanring here from the asset.
                    if (flyingText == null) {
                        var fto = UnityEngine.GameObject.Instantiate(SceneGraph.Current.InternalResources.FlyingTextObject, SceneGraph.Current.transform);
                        fto.name = "Tweedle Flying Text";
                    }
                    entity = SGEntity.Create<SGTextModel>(model);
                    SGTextModel textModel = entity as SGTextModel;

                    break;
                default:
                    var jointedEntity = SGEntity.Create<SGJointedModel>(model);
                    jointedEntity.SetResource(resource);
                    entity = jointedEntity;
                    break;
            }

            SceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void createJointEntity(TValue joint, string id, TValue model) {
            CreateJoint(joint, id, model);
        }

        private static SGJoint CreateJoint(TValue joint, string id, TValue model) {
            var modelEnt = SceneGraph.Current.FindEntity<SGJointedModel>(model);
            if (!modelEnt) return null;
            var jointEnt = modelEnt.LinkJoint(joint, id);
            if (jointEnt) {
                SceneGraph.Current.AddEntity(jointEnt);
            }
            return jointEnt;
        }

        [PInteropMethod]
        public static string[] findJointsBeginningWith(string start, TValue model) {
            var modelEnt = SceneGraph.Current.FindEntity<SGJointedModel>(model);
            if (modelEnt) {
                return modelEnt.FindJointsBeginningWith(start);
            }
            throw new SceneGraphException("No scene graph entity exists for tweedle object.");
        }

        [PInteropMethod]
        public static void createSceneEntity(TValue scene) {
            var entity = SGEntity.Create<SGScene>(scene);
            SceneGraph.Current.AddEntity(entity);
            SceneGraph.Current.Scene = entity;
        }

        [PInteropMethod]
        public static void createCameraEntity(TValue camera) {
            var entity = SGEntity.Create<SGCamera>(camera);
            SceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void createVrHandEntity(TValue hand) {
            var entity = SGEntity.Create<SGVRHand>(hand);
            SceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void connectVrHandToCamera(TValue hand, TValue camera) {
            var hnd = SceneGraph.Current.FindEntity<SGVRHand>(hand);
            var cam = SceneGraph.Current.FindEntity<SGCamera>(camera);
            hnd.SetCamera(cam);
        }

        [PInteropMethod]
        public static void createGroundEntity(TValue ground) {
            var entity = SGEntity.Create<SGGround>(ground);
            SceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void createRoomEntity(TValue room) {
            var entity = SGEntity.Create<SGRoom>(room);
            SceneGraph.Current.AddEntity(entity);
        }

        [PInteropMethod]
        public static void createMarkerEntity(TValue marker) {
            var entity = SGEntity.Create<SGMarker>(marker);
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
        public static void bindBackPaintProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGModel.BACK_PAINT_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindFloorPaintProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGRoom.FLOOR_PAINT_PROPERTY_NAME, owner, property, value);
        }

         [PInteropMethod]
        public static void bindCeilingPaintProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGRoom.CEILING_PAINT_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindRadiusProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGShape.RADIUS_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindInnerRadiusProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGShape.INNER_RADIUS_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindOuterRadiusProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGShape.OUTER_RADIUS_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void bindLengthProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGShape.LENGTH_PROPERTY_NAME, owner, property, value);
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
        public static void bindTextProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.BindProperty(SGTextModel.TEXT_PROPERTY_NAME, owner, property, value);
        }

        [PInteropMethod]
        public static void updateProperty(TValue owner, TValue property, TValue value) {
            SceneGraph.Current.UpdateProperty(owner, property, value);
        }
        #endregion // Property Binding

        #region Camera
        [PInteropMethod]
        public static void setCameraVerticalFOV(TValue camera, Angle fov) {
            var cam = SceneGraph.Current.FindEntity<SGCamera>(camera);
            if (cam) {
                cam.Camera.fieldOfView = (float)fov.degrees;
            }
        }

        [PInteropMethod]
        public static void setCameraClipPlanes(TValue camera, double nearClip, double farClip) {
            var cam = SceneGraph.Current.FindEntity<SGCamera>(camera);
            if (cam) {
                cam.Camera.nearClipPlane = (float)nearClip;
                cam.Camera.farClipPlane = (float)farClip;
            }
        }
        #endregion // Camera

        #region Other

        [PInteropMethod]
        public static void setEntityName(TValue thing, string name) {
            var entity = SceneGraph.Current.FindEntity(thing);
            entity?.SetName(name);
        }

        [PInteropMethod]
        public static void setEntityResource(TValue model, string resource) {
            var modelEnt = SceneGraph.Current.FindEntity<SGJointedModel>(model);
            if (modelEnt) {
                modelEnt.SetResource(resource);
            }
        }

        [PInteropMethod]
        public static VantagePoint setVehicle(TValue vehicle, TValue rider) {
            var entity = SceneGraph.Current.FindEntity(rider);
            if (entity) {
                entity.vehicle = SceneGraph.Current.FindEntity(vehicle);
                
                var p = entity.cachedTransform.localPosition;
                var r = entity.cachedTransform.localRotation;
                return VantagePoint.FromUnity(p, r);
            } else {
                throw new SceneGraphException("No scene graph entity exists for tweedle object.");
            }
        }

        [PInteropMethod]
        public static TValue getVehicle(TValue rider) {
            var riderEnt = SceneGraph.Current.FindEntity(rider);
            return riderEnt?.vehicle == null ? TValue.NULL : riderEnt.vehicle.owner;
        }

        [PInteropMethod]
        public static AsyncReturn say(TValue entity, string bubbleText, int bubblePosition, 
                                    int fontType, int textStyle, double textScale, TValue bubbleColor, 
                                    TValue outlineColor, TValue textColor, Duration duration) {
            var bubbleColorRaw = bubbleColor.RawObject<Primitives.Color>().Value;
            UnityEngine.Color bubbleColorConverted = new UnityEngine.Color((float)bubbleColorRaw.R, (float)bubbleColorRaw.G, (float)bubbleColorRaw.B, (float)bubbleColorRaw.A);
            
            var outlineColorRaw = outlineColor.RawObject<Primitives.Color>().Value;
            UnityEngine.Color outlineColorConverted = new UnityEngine.Color((float)outlineColorRaw.R, (float)outlineColorRaw.G, (float)outlineColorRaw.B, (float)outlineColorRaw.A);
            
            var textColorRaw = textColor.RawObject<Primitives.Color>().Value;
            UnityEngine.Color textColorConverted = new UnityEngine.Color((float)textColorRaw.R, (float)textColorRaw.G, (float)textColorRaw.B, (float)textColorRaw.A);

            AsyncReturn asyncReturn = new AsyncReturn();

            SceneCanvas canvas = SceneGraph.Current.GetCurrentCanvas();
            var sgEntity = SceneGraph.Current.FindEntity(entity);
            canvas.SayThinkControl.SpawnSayThink(asyncReturn, canvas.transform, sgEntity, bubbleText, true, 
                                                (BubblePosition)bubblePosition, (FontType) fontType, (TextStyle) textStyle, (float)textScale, 
                                                bubbleColorConverted, outlineColorConverted, textColorConverted, duration.Value);
        
            
            return asyncReturn;
        }

        [PInteropMethod]
        public static AsyncReturn think(TValue entity, string bubbleText, int bubblePosition, 
                                    int fontType, int textStyle, double textScale, TValue bubbleColor, 
                                    TValue outlineColor, TValue textColor, Duration duration) {
            var bubbleColorRaw = bubbleColor.RawObject<Primitives.Color>().Value;
            UnityEngine.Color bubbleColorConverted = new UnityEngine.Color((float)bubbleColorRaw.R, (float)bubbleColorRaw.G, (float)bubbleColorRaw.B, (float)bubbleColorRaw.A);
            
            var outlineColorRaw = outlineColor.RawObject<Primitives.Color>().Value;
            UnityEngine.Color outlineColorConverted = new UnityEngine.Color((float)outlineColorRaw.R, (float)outlineColorRaw.G, (float)outlineColorRaw.B, (float)outlineColorRaw.A);
            
            var textColorRaw = textColor.RawObject<Primitives.Color>().Value;
            UnityEngine.Color textColorConverted = new UnityEngine.Color((float)textColorRaw.R, (float)textColorRaw.G, (float)textColorRaw.B, (float)textColorRaw.A);

            AsyncReturn asyncReturn = new AsyncReturn();

            SceneCanvas canvas = SceneGraph.Current.GetCurrentCanvas();
            var sgEntity = SceneGraph.Current.FindEntity(entity);
            canvas.SayThinkControl.SpawnSayThink(asyncReturn, canvas.transform, sgEntity, bubbleText, false, 
                                                (BubblePosition)bubblePosition, (FontType) fontType, (TextStyle) textStyle, (float)textScale, 
                                                bubbleColorConverted, outlineColorConverted, textColorConverted, duration.Value);
        
            
            return asyncReturn;
        }

        [PInteropMethod]
        public static AsyncReturn playAudio(TValue entity, string sound, float volume, float startTime, float stopTime)
        {
            var asyncReturn = new AsyncReturn();
            var entityXform = SceneGraph.Current.FindEntity(entity);
            if (entityXform == null) {
                return asyncReturn;
            }
            // Create a new audio source every time, because we need to use the start time and stop time, so PlayOneShot will not work. We will destroy the audioclip when it's finished playing.
            TweedleAudioPlayer audio = UnityEngine.GameObject.Instantiate(SceneGraph.Current.InternalResources.TweedleAudioSource, entityXform.transform);
            AudioClip clip = SceneGraph.Current.AudioCache.Get(sound);
            float waitTime = stopTime < 0f ? clip.length : (stopTime - startTime);
            audio.SetData(clip, volume, startTime);
            audio.Play(waitTime);
            SceneGraph.Current.QueueTimeReturn(asyncReturn, waitTime);
            return asyncReturn;
        }

        [PInteropMethod]
        public static AsyncReturn<bool> doTheseCollide(TValue targetA, TValue targetB)
        {
            AsyncReturn<bool> asyncReturn = new AsyncReturn<bool>();
            var entityA = SceneGraph.Current.FindEntity(targetA);
            var entityB = SceneGraph.Current.FindEntity(targetB);
            bool AisModel = false;
            bool BisModel = false;
            Bounds entityABounds = new Bounds();
            Bounds entityBBounds = new Bounds();

            if (entityA is SGModel){
                AisModel = true;
                entityABounds = (entityA as SGModel).GetBoundsInWorldSpace(false);
            }
            if (entityB is SGModel){
                BisModel = true;
                entityBBounds = (entityB as SGModel).GetBoundsInWorldSpace(false);
            }

            if(AisModel && BisModel){
                asyncReturn.Return(entityABounds.Intersects(entityBBounds));
            }
            else if(AisModel){
                asyncReturn.Return(entityABounds.Contains(entityB.cachedTransform.position));
            }
            else if(BisModel){
                asyncReturn.Return(entityBBounds.Contains(entityA.cachedTransform.position));
            }
            else{ // Neither entity is a model. We could check if positions match exactly or within some small delta
                asyncReturn.Return(UnityEngine.Vector3.Distance(entityA.cachedTransform.position, entityB.cachedTransform.position) < 0.05f);
            }

            return asyncReturn;
        }

        private static IEnumerator DelayReturnRoutine(AsyncReturn asyncReturn, float delay)
        {
            yield return delay;
            asyncReturn.Return();
        }

        [PInteropMethod]
        public static void updateEyesOnChange(TValue eye, TValue person) {
            var entity = SceneGraph.Current.FindEntity<SGJointedModel>(person);
            var eyeJoint = SceneGraph.Current.FindEntity<SGJoint>(eye);
            entity.WatchEye(eyeJoint);
        }

        [PInteropMethod]
        public static void updateMouthBlendOnChange(TValue mouth, TValue person) {
            var entity = SceneGraph.Current.FindEntity<SGJointedModel>(person);
            var mouthJoint = SceneGraph.Current.FindEntity<SGJoint>(mouth);
            entity.WatchMouth(mouthJoint);
        }

        [PInteropMethod]
        public static void updateEyelidBlendsOnChange(TValue eyelid, TValue person) {
            var entity = SceneGraph.Current.FindEntity<SGJointedModel>(person);
            var eyelidJoint = SceneGraph.Current.FindEntity<SGJoint>(eyelid);
            entity.WatchEyelid(eyelidJoint);
        }
        #endregion // Other

        #region Events
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
        #endregion // Events

        #region Transformations
        [PInteropMethod]
        public static VantagePoint getLocalTransformation(TValue thing) {
            var entity = SceneGraph.Current.FindEntity(thing);
            return entity ? entity.GetLocalTransformation() : VantagePoint.IDENTITY;
        }

        [PInteropMethod]
        public static VantagePoint getAbsoluteTransformation(TValue thing) {
            var entity = SceneGraph.Current.FindEntity(thing);
            return entity ? entity.GetAbsoluteTransformation() : VantagePoint.IDENTITY;
        }

        [PInteropMethod]
        public static VantagePoint getInverseAbsoluteTransformation(TValue thing) {
            var entity = SceneGraph.Current.FindEntity(thing);
            if (entity) {
                var r = UnityEngine.Quaternion.Inverse(entity.cachedTransform.rotation);
                r.Normalize();
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
            }
            throw new SceneGraphException("No scene graph entity exists for tweedle object.");
        }

        [PInteropMethod]
        public static AxisAlignedBox getLocalBoundingBox(TValue model, bool dynamic = false) {
            var entity = SceneGraph.Current.FindEntity(model);
            if (entity && entity is SGModel) {
                var sgModel = (SGModel)entity;
                return AxisAlignedBox.FromUnity(sgModel.GetBounds(dynamic));
            }
            throw new SceneGraphException("No scene graph entity exists for tweedle object.");
        }

        [PInteropMethod]
        public static Size getLocalBoundingBoxSize(TValue model, bool dynamic = false) {
            var entity = SceneGraph.Current.FindEntity(model);
            if (entity && entity is SGModel) {
                var sgModel = (SGModel)entity;
                var size = sgModel.GetSize(dynamic);
                return new Size(size.x, size.y, size.z);
            }
            throw new SceneGraphException("No scene graph entity exists for tweedle object.");
        }

        [PInteropMethod]
        public static Scale getLocalScale(TValue model)
        {
            var entity = SceneGraph.Current.FindEntity(model);
            if (entity && entity is SGModel)
            {
                var sgModel = (SGModel)entity;
                return sgModel.GetScale();
            }
            throw new SceneGraphException("No scene graph entity exists for tweedle object.");
        }
        #endregion // Transformations
    }
}
