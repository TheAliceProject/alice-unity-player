using UnityEngine;
using Alice.Tweedle;
using Alice.Tweedle.Interop;
using System;
using System.Collections.Generic;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using BeauRoutine;

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
        private List<MouseEventListenerProxy> m_MouseClickListeners = new List<MouseEventListenerProxy>();

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
        private float dragSpeed = 10f;
        private UnityEngine.Vector3 dragOrigin;
        private UnityEngine.Vector3 shiftOrigin;
        private UnityEngine.Vector3 rotateOrigin;

        private float lastMouseDownTime = 0f;
        private Transform objectToMove = null;
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
        }

        // Time, Mouse, and Keyboard intercepting
        void Update(){
            CheckTimeListeners();
            CheckMouseListeners();
            CheckKeyboardListeners();
        }

        private void CheckTimeListeners()
        {
            for (int i = 0; i < m_TimeListeners.Count; i++){
                m_TimeListeners[i].CheckEvent();
            }
        }

        private void CheckMouseListeners()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)){ // Left mouse click
                lastMouseDownTime = Time.time;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Draw ray from screen to mouse click point
                for (int i = 0; i < m_MouseClickListeners.Count; i++){
                    if (Physics.Raycast(ray, out hit, 100.0f)){
                        Debug.Log("You selected the " + hit.transform.parent.name); // ensure you picked right object
                        if (defaultModelManipulationActive){
                            objectToMove = hit.transform.GetComponentInParent<SGModel>().transform;  // transform.parent;
                            objectOriginPoint = hit.transform.position;
                            Ray planeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                            float distance;
                            if (movementPlane.Raycast(planeRay, out distance))
                                planeOriginPoint = planeRay.origin + (planeRay.direction * distance);
                        }
                    }
                }
            }

            if (GetShiftDown())
                shiftOrigin = Input.mousePosition;
            if (GetCtrlDown())
                rotateOrigin = Input.mousePosition;
            if (Input.GetKeyUp(KeyCode.Mouse0)){
                objectToMove = null;
                if (Time.time - lastMouseDownTime < 0.25f){ // Considered a click and not a hold
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Draw ray from screen to mouse click point
                    Portion distanceFromLeft = new Portion((float)Input.mousePosition.x / (float)Screen.width);
                    Portion distanceFromBottom = new Portion((float)Input.mousePosition.y / (float)Screen.height);
                    for (int i = 0; i < m_MouseClickListeners.Count; i++){
                        if (m_MouseClickListeners[i].onlyOnModels){ // Clicked on object event
                            if (Physics.Raycast(ray, out hit, 100.0f)){
                                if (m_MouseClickListeners[i].targets.Length == 0){ // They didn't specify visuals, so call event because we hit something
                                    m_MouseClickListeners[i].CallEvent(distanceFromBottom, distanceFromLeft, hit.transform.GetComponentInParent<SGModel>().owner);
                                }
                                else{  // Make sure what we clicked on is in the list of visuals
                                    for (int j = 0; j < m_MouseClickListeners[i].targets.Length; j++){
                                        if (m_MouseClickListeners[i].targets[j] == hit.transform.GetComponentInParent<SGModel>().transform.gameObject){
                                            m_MouseClickListeners[i].CallEvent(distanceFromBottom, distanceFromLeft, hit.transform.GetComponentInParent<SGModel>().owner);
                                        }
                                    }
                                }
                            }
                        }
                        else{ // Clicked on screen event
                            m_MouseClickListeners[i].CallEvent(distanceFromBottom, distanceFromLeft);
                        }
                    }
                }
            }


            if (defaultModelManipulationActive && (objectToMove != null) && (GetShiftUp() || GetCtrlUp())){
                objectOriginPoint = objectToMove.position;
                Ray planeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance;
                if (movementPlane.Raycast(planeRay, out distance))
                    planeOriginPoint = planeRay.origin + (planeRay.direction * distance);
            }

            if (Input.GetMouseButtonDown(0)){
                dragOrigin = Input.mousePosition;
                return;
            }

            // After this point do nothing if mouse button is not held
            if (!Input.GetMouseButton(0))
                return;


            if (objectToMove == null && defaultModelManipulationActive){
                objectToMove = Camera.main.transform;
            }

            if (objectToMove == Camera.main.transform){ // Moving the camera
                if (GetShiftDown(true)){    // Up down
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - shiftOrigin);
                    UnityEngine.Vector3 move = new UnityEngine.Vector3(pos.x * (1f * dragSpeed), dragSpeed * pos.y, pos.y * (1f * dragSpeed));
                    objectToMove.position += move;
                    shiftOrigin = Input.mousePosition;
                }
                else if (GetCtrlDown(true)){ // Rotate
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - rotateOrigin);
                    objectToMove.Rotate(UnityEngine.Vector3.up, dragSpeed * pos.x * 20f);
                    rotateOrigin = Input.mousePosition;
                }
                else{   // Scroll
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                    UnityEngine.Vector3 move = new UnityEngine.Vector3(pos.x * (1f * dragSpeed), 0, pos.y * (1f * dragSpeed));
                    objectToMove.position += move;
                    dragOrigin = Input.mousePosition;
                }
            }
            else if (objectToMove != null){ // Moving an object
                // If holding shift, move object up and down
                if (GetShiftDown(true)){
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - shiftOrigin);
                    UnityEngine.Vector3 move = new UnityEngine.Vector3(0f, dragSpeed * pos.y, 0f);
                    objectToMove.position += move;
                    shiftOrigin = Input.mousePosition;
                }
                else if (GetCtrlDown(true)){ // If holding control, rotate object
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - rotateOrigin);
                    objectToMove.Rotate(UnityEngine.Vector3.up, dragSpeed * pos.x * 200f);
                    rotateOrigin = Input.mousePosition;
                }
                else{ // move object along plane
                    Ray planeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float distance;
                    if (movementPlane.Raycast(planeRay, out distance)){
                        UnityEngine.Vector3 pointalongplane = planeRay.origin + (planeRay.direction * distance);
                        UnityEngine.Vector3 moveAmount = planeOriginPoint - pointalongplane;
                        objectToMove.position = new UnityEngine.Vector3(objectOriginPoint.x - moveAmount.x, objectToMove.position.y, objectOriginPoint.z - moveAmount.z);
                    }
                }

            }
        }

        private void CheckKeyboardListeners()
        {
            // ToDo
            return;
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
            m_MouseClickListeners.Add(new MouseEventListenerProxy(inListener, eventPolicy));
        }

        public void AddMouseClickOnObjectListener(PAction<Primitives.Portion, Primitives.Portion, TValue> inListener, OverlappingEventPolicy eventPolicy, SGModel[] clickedObjects) {
            AddMouseColliders(clickedObjects);
            m_MouseClickListeners.Add(new MouseEventListenerProxy(inListener, eventPolicy, true, clickedObjects));
        }

        public void AddMouseColliders(SGModel[] models)
        {
            for (int i = 0; i < models.Length; i++)
            {
                foreach (MeshRenderer renderer in models[i].transform.GetComponentsInChildren<MeshRenderer>())
                {
                    if (renderer.transform.GetComponent<MeshCollider>() == null)
                    {
                        MeshCollider collider = renderer.gameObject.AddComponent<MeshCollider>();
                        collider.convex = true;
                    }
                }
                foreach (SkinnedMeshRenderer renderer in models[i].transform.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (renderer.transform.GetComponent<MeshCollider>() == null)
                    {
                        MeshCollider collider = renderer.gameObject.AddComponent<MeshCollider>();
                        collider.convex = true;
                    }
                }
            }
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

        private bool GetShiftDown(bool hold=false)
        {
            if(hold)
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            else
                return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        }

        private bool GetCtrlDown(bool hold=false)
        {
            if(hold)
                return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            else
                return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
        }

        private bool GetShiftUp()
        {
            return Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift);
        }

        private bool GetCtrlUp()
        {
            return Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl);
        }
    }
}