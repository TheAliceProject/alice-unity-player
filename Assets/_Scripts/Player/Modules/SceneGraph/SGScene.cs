using UnityEngine;
using Alice.Tweedle;
using Alice.Tweedle.Interop;
using System;
using System.Collections.Generic;
using Alice.Player.Modules;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    
    public sealed class SGScene : SGEntity {

        public const string FOG_DENSITY_PROPERTY_NAME = "FogDensity";
        public const string ATMOSPHERE_COLOR_PROPERTY_NAME = "AtmosphereColor";
        public const string GLOBAL_BRIGHTNESS_PROPERTY_NAME = "Brightness";
        public const string AMBIENT_LIGHT_COLOR_PROPERTY_NAME = "AmbientLightColor";
        public const string ABOVE_LIGHT_COLOR_PROPERTY_NAME = "AboveLightColor";
        public const string BELOW_LIGHT_COLOR_PROPERTY_NAME = "BelowLightColor";

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
            for(int i = 0; i < m_TimeListeners.Count; i++){
                    m_TimeListeners[i].CheckEvent();
            }

            if(Input.GetKeyDown(KeyCode.Mouse0)){
                Debug.Log("Mouse down");
                for(int i = 0; i < m_MouseClickListeners.Count; i++){
                    m_MouseClickListeners[i].CallEvent();
                }
            }
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

        public void AddMouseClickListener(PAction inListener, OverlappingEventPolicy eventPolicy) {
            m_MouseClickListeners.Add(new MouseEventListenerProxy(inListener, eventPolicy));
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