using UnityEngine;
using Alice.Tweedle;
using Alice.Tweedle.Interop;
using System;
using System.Collections.Generic;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using BeauRoutine;
using UnityEngine.XR;

namespace Alice.Player.Unity {
    
    public sealed class SGScene : SGEntity {

        public const string FOG_DENSITY_PROPERTY_NAME = "FogDensity";
        public const string ATMOSPHERE_COLOR_PROPERTY_NAME = "AtmosphereColor";
        public const string GLOBAL_BRIGHTNESS_PROPERTY_NAME = "Brightness";
        public const string AMBIENT_LIGHT_COLOR_PROPERTY_NAME = "AmbientLightColor";
        public const string ABOVE_LIGHT_COLOR_PROPERTY_NAME = "AboveLightColor";
        public const string BELOW_LIGHT_COLOR_PROPERTY_NAME = "BelowLightColor";
        public static bool defaultModelManipulationActive = false;
        private List<PAction> m_ActivationListeners = new List<PAction>();
        private List<TimeEventListenerProxy> m_TimeListeners = new List<TimeEventListenerProxy>();
        private KeyboardEventHandler m_KeyboardEventHandler = new KeyboardEventHandler();
        private MouseEventHandler m_MouseEventHandler = new MouseEventHandler();
        private VREventHandler m_VrEventHandler = new VREventHandler();
        private InteractionEventHandler m_InteractionHandler = new InteractionEventHandler();

        private UnityEngine.Color m_AmbientLightColor = new UnityEngine.Color(0.25f, 0.25f, 0.25f, 1f);
        private UnityEngine.Color m_AtmosphereColor = UnityEngine.Color.white;
        private float m_GlobalBrightness = 1f;

        private SceneCanvas m_SceneCanvas;
        private Light m_AboveLightA;
        private Light m_AboveLightB;
        private Light m_AboveLightC;
        private const float k_AboveLightIntensity = 0.533f;
        private const float k_AboveLightPitch = 45f;
        private Light m_BelowLight;
        private const float k_BelowLightIntensity = 1f;
        private const float k_BelowLightPitch = -90f;
        private UnityEngine.Vector3 m_DefaultColliderSize = new UnityEngine.Vector3(0.1f, 0.1f, 0.1f);

        private UnityEngine.Vector3 dragOrigin;
        private UnityEngine.Vector3 shiftOrigin;
        private UnityEngine.Vector3 rotateOrigin;

        private Plane movementPlane = new Plane(UnityEngine.Vector3.up, UnityEngine.Vector3.zero);
        private UnityEngine.Vector3 objectOriginPoint = UnityEngine.Vector3.zero;
        private UnityEngine.Vector3 planeOriginPoint = UnityEngine.Vector3.zero;

        protected override void Awake() {
            base.Awake();

            m_AboveLightA = CreateLight(k_AboveLightPitch, 0f, k_AboveLightIntensity, true);
            m_AboveLightB = CreateLight(k_AboveLightPitch, 120f, k_AboveLightIntensity, false);
            m_AboveLightC = CreateLight(k_AboveLightPitch, 240f, k_AboveLightIntensity, false);

            m_BelowLight = CreateLight(k_BelowLightPitch, 0, k_BelowLightIntensity, false);

            m_SceneCanvas = CreateCanvas();
            RenderSettings.fogMode = FogMode.Linear;

            RegisterPropertyDelegate(FOG_DENSITY_PROPERTY_NAME, OnUpdateFogDensity);
            RegisterPropertyDelegate(ATMOSPHERE_COLOR_PROPERTY_NAME, OnUpdateAtmosphereColor);
            RegisterPropertyDelegate(GLOBAL_BRIGHTNESS_PROPERTY_NAME, OnUpdateGlobalBrightness);
            RegisterPropertyDelegate(AMBIENT_LIGHT_COLOR_PROPERTY_NAME, OnUpdateAmbientLightColor);
            RegisterPropertyDelegate(ABOVE_LIGHT_COLOR_PROPERTY_NAME, OnUpdateAboveLightColor);
            RegisterPropertyDelegate(BELOW_LIGHT_COLOR_PROPERTY_NAME, OnUpdateBelowLightColor);
            m_MouseEventHandler.isMac = SystemInfo.operatingSystem.Contains("Mac OS");
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus)
                m_KeyboardEventHandler.RemoveAllKeys();
        }

        // Time, Mouse, and Keyboard intercepting
        void Update(){
            CheckTimeListeners();
            m_MouseEventHandler.HandleMouseEvents(); 
            m_KeyboardEventHandler.HandleKeyboardEvents();
            m_InteractionHandler.HandleInteractionEvents();
            m_VrEventHandler.HandleVREvents();
        }

        private void CheckTimeListeners()
        {
            for (int i = 0; i < m_TimeListeners.Count; i++){
                m_TimeListeners[i].CheckEvent();
            }
        }

        public void SetDefaultModelManipulationActive(bool active)
        {
            if (XRSettings.enabled)
                VRControl.I.rig.EnableLaserPointers(true);
            m_MouseEventHandler.SetModelManipulation(active);
        }
    
        private Light CreateLight(float inPitch, float inHeading, float intensity, bool useShadows) {
            var light = new GameObject("Light").AddComponent<Light>();
            light.transform.parent = cachedTransform;
            light.type = LightType.Directional;
            light.shadows = useShadows ? LightShadows.Hard : LightShadows.None;
            light.shadowStrength = 0.8f;
            light.transform.localRotation = UnityEngine.Quaternion.Euler(inPitch, inHeading, 0);
            light.intensity = intensity;

            return light;
        }

        private SceneCanvas CreateCanvas()
        {
            var canvas = Instantiate(SceneGraph.Current.InternalResources.SceneCanvas);
            canvas.transform.SetParent(cachedTransform);
            return canvas;
        }

        public SceneCanvas GetCurrentCanvas()
        {
            return m_SceneCanvas;
        }
        
        public void  AddActivationListener(PAction inListener) {
            m_ActivationListeners.Add(inListener);
        }

        public void AddTimeListener(PAction<Duration> inListener, float frequency, OverlappingEventPolicy eventPolicy) {
            m_TimeListeners.Add(new TimeEventListenerProxy(inListener, frequency, eventPolicy));
        }

        public void AddMouseClickOnScreenListener(PAction<Primitives.Portion, Primitives.Portion> inListener, OverlappingEventPolicy eventPolicy) {
            m_MouseEventHandler.AddMouseListener(new MouseEventListenerProxy(inListener, eventPolicy));
        }

        public void AddMouseClickOnObjectListener(PAction<Primitives.Portion, Primitives.Portion, TValue> inListener, OverlappingEventPolicy eventPolicy, SGModel[] clickedObjects) {
            if (XRSettings.enabled)
                VRControl.I.rig.EnableLaserPointers(true);
            AddColliders(clickedObjects);
            m_MouseEventHandler.AddMouseListener(new MouseEventListenerProxy(inListener, eventPolicy, clickedObjects));
        }

        public void AddKeyListener(PAction<int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy)
        {
            m_KeyboardEventHandler.AddListener(new KeyEventListenerProxy(listener, overlappingEventPolicy, heldKeyPolicy, KeyEventListenerProxy.KeyPressType.Normal));
        }

        public void AddArrowKeyListener(PAction<int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy)
        {
            m_KeyboardEventHandler.AddListener(new KeyEventListenerProxy(listener, overlappingEventPolicy, heldKeyPolicy, KeyEventListenerProxy.KeyPressType.ArrowKey));
        }

        public void AddNumberKeyListener(PAction<int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy)
        {
            m_KeyboardEventHandler.AddListener(new KeyEventListenerProxy(listener, overlappingEventPolicy, heldKeyPolicy, KeyEventListenerProxy.KeyPressType.NumPadKey));
        }

        public void AddKeyMover(SGTransformableEntity entity)
        {
            m_KeyboardEventHandler.AddObjectMover(entity);
        }

        public void AddCollisionListener(PAction<TValue, TValue> listener, OverlappingEventPolicy overlappingEventPolicy, SGEntity[] a, SGEntity[] b, InteractionModule.InteractionType interactionType)
        {
            m_InteractionHandler.AddCollisionListener(new CollisionEventListenerProxy(listener, overlappingEventPolicy, a, b, interactionType));
        }

        public void AddViewListener(PAction<TValue> listener, OverlappingEventPolicy overlappingEventPolicy, SGModel[] set, InteractionModule.InteractionType interactionType)
        {
            m_InteractionHandler.AddViewListener(new ViewEventListenerProxy(listener, overlappingEventPolicy, set, interactionType));
        }

        public void AddPointOfViewChangeListener(PAction<TValue> listener, SGEntity[] set)
        {
            m_InteractionHandler.AddPointOfViewChangeListener(new PointOfViewChangeEventListenerProxy(listener, set));
        }

        public void AddProximityListener(PAction<TValue, TValue> listener, OverlappingEventPolicy overlappingEventPolicy, SGEntity[] a, SGEntity[] b, float distance, InteractionModule.InteractionType interactionType)
        {
            m_InteractionHandler.AddProximityListener(new ProximityEventListenerProxy(listener, overlappingEventPolicy, a, b, distance, interactionType));
        }

        public void AddOcclusionListener(PAction<TValue, TValue> listener, OverlappingEventPolicy overlappingEventPolicy, SGModel[] setA, SGModel[] setB, InteractionModule.InteractionType interactionType)
        {
            m_InteractionHandler.AddOcclusionListener(new OcclusionEventListenerProxy(listener, overlappingEventPolicy, setA, setB, interactionType));
        }
        
        public void AddColliders(SGEntity[] models)
        {
            for (int i = 0; i < models.Length; i++)
            {
                MeshRenderer[] meshRenderers = models[i].transform.GetComponentsInChildren<MeshRenderer>();
                SkinnedMeshRenderer[] skinnedMeshRenderers = models[i].transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                
                if((meshRenderers != null || skinnedMeshRenderers != null) && (meshRenderers.Length > 0 || skinnedMeshRenderers.Length > 0))
                {
                    foreach (MeshRenderer renderer in meshRenderers)
                    {
                        if (renderer.transform.GetComponent<MeshCollider>() == null)
                        {
                            MeshCollider collider = renderer.gameObject.AddComponent<MeshCollider>();
                            Rigidbody rigidBody = renderer.gameObject.AddComponent<Rigidbody>();    // Rigidbody is required for collision detection
                            rigidBody.isKinematic = true;
                            collider.convex = true;
                            collider.isTrigger = true;
                            renderer.gameObject.AddComponent<CollisionBroadcaster>();
                        }
                    }
                    foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
                    {
                        if (renderer.transform.GetComponent<MeshCollider>() == null)
                        {
                            MeshCollider collider = renderer.gameObject.AddComponent<MeshCollider>();
                            Rigidbody rigidBody = renderer.gameObject.AddComponent<Rigidbody>();
                            rigidBody.isKinematic = true;

                            Mesh colliderMesh = new Mesh();
                            renderer.BakeMesh(colliderMesh);
                            collider.sharedMesh = null;
                            collider.sharedMesh = colliderMesh;
                            
                            collider.convex = true;
                            collider.isTrigger = true;
                            renderer.gameObject.AddComponent<CollisionBroadcaster>();
                        }
                    }
                }
                else // Add box collider if no renderer exists
                {
                    if(models[i].gameObject.GetComponent<BoxCollider>() == null)
                    {
                        Rigidbody rigidBody = models[i].gameObject.AddComponent<Rigidbody>();
                        rigidBody.isKinematic = true;
                        BoxCollider collider = models[i].gameObject.AddComponent<BoxCollider>();
                        collider.size = m_DefaultColliderSize;
                        collider.isTrigger = true;
                        models[i].gameObject.AddComponent<CollisionBroadcaster>();
                    }
                }
            }
        }

        public void AddViewEnterBroadcasters(SGModel[] models)
        {
            for (int i = 0; i < models.Length; i++){
                MeshRenderer[] meshRenderers = models[i].transform.GetComponentsInChildren<MeshRenderer>();
                SkinnedMeshRenderer[] skinnedMeshRenderers = models[i].transform.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (MeshRenderer renderer in meshRenderers){
                    if (renderer.transform.GetComponent<ViewEnterBroadcaster>() == null){
                        renderer.gameObject.AddComponent<ViewEnterBroadcaster>();
                    }
                }
                foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers){
                    if (renderer.transform.GetComponent<ViewEnterBroadcaster>() == null){
                        renderer.gameObject.AddComponent<ViewEnterBroadcaster>();
                    }
                }
            }

        }

        public void ObjectsCollided(SGEntity firstObject, SGEntity secondObject, bool enter)
        {
            m_InteractionHandler.NotifyObjectsCollided(firstObject, secondObject, enter);
        }

        public void ObjectsOccluded(SGModel foregroundObject, SGModel backgroundObject)
        {
            m_InteractionHandler.NotifyObjectsOccluded(foregroundObject, backgroundObject);
        }

        public void ObjectInView(SGModel model, bool enteredView)
        {
            m_InteractionHandler.NotifyModelInView(model, enteredView);
        }

        public void Activate() {
            for (int i = 0, count = m_ActivationListeners.Count; i < count; ++i) {
                m_ActivationListeners[i].Call();
            }
        }

        private void OnUpdateFogDensity(TValue inValue) {
            var density = (float)inValue.RawObject<Primitives.Portion>().Value;
            RenderSettings.fogDensity = density * density * density; // alice is cubing the density to give some more control
            RenderSettings.fog = density > float.Epsilon;
        }

        private void OnUpdateAtmosphereColor(TValue inValue) {
            var color = inValue.RawObject<Primitives.Color>().Value;
            m_AtmosphereColor = new UnityEngine.Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
            
            UpdateAtmosphereColor();
        }

        private void OnUpdateGlobalBrightness(TValue inValue) {
            var brightness = (float)inValue.RawObject<Primitives.Portion>().Value;
            m_GlobalBrightness = brightness;
            //RenderSettings.ambientIntensity = brightness;
            RenderSettings.reflectionIntensity = brightness;
            m_AboveLightA.intensity = m_AboveLightB.intensity = m_AboveLightC.intensity = k_AboveLightIntensity * brightness;
            m_BelowLight.intensity = k_BelowLightIntensity * brightness;

            UpdateAtmosphereColor();
            UpdateAmbientLightColor();
        }

        private void OnUpdateAmbientLightColor(TValue inValue) {
            var color = inValue.RawObject<Primitives.Color>().Value;
            m_AmbientLightColor = new UnityEngine.Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
            UpdateAmbientLightColor();
        }

        private void OnUpdateAboveLightColor(TValue inValue) {
            var aliceColor = inValue.RawObject<Primitives.Color>().Value;
            var unityColor = new UnityEngine.Color((float)aliceColor.R, (float)aliceColor.G, (float)aliceColor.B, (float)aliceColor.A);
            m_AboveLightA.color = unityColor;
            m_AboveLightB.color = unityColor;
            m_AboveLightC.color = unityColor;
        }

        private void OnUpdateBelowLightColor(TValue inValue) {
            var color = inValue.RawObject<Primitives.Color>().Value;
            m_BelowLight.color = new UnityEngine.Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
        }

        private void UpdateAtmosphereColor() {
            Camera.main.backgroundColor = RenderSettings.fogColor = UnityEngine.Color.Lerp(UnityEngine.Color.clear, 
                                                                               m_AtmosphereColor, 
                                                                               m_GlobalBrightness);
        }   

        private void UpdateAmbientLightColor() {
            RenderSettings.ambientLight = UnityEngine.Color.Lerp(new UnityEngine.Color(0,0,0,1), 
                                                     m_AmbientLightColor, 
                                                     m_GlobalBrightness);
        }

        public override void CleanUp() {

        }
    }
}