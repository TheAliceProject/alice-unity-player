using UnityEngine;
using Alice.Tweedle;
using Alice.Tweedle.Interop;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public sealed class SGScene : SGEntity {

        public const string FOG_DENSITY_PROPERTY_NAME = "FogDensity";
        public const string ATMOSPHERE_COLOR_PROPERTY_NAME = "AtmosphereColor";
        public const string GLOBAL_BRIGHTNESS_PROPERTY_NAME = "Brightness";
        public const string AMBIENT_LIGHT_COLOR_PROPERTY_NAME = "AmbientLightColor";
        public const string ABOVE_LIGHT_COLOR_PROPERTY_NAME = "AboveLightColor";
        public const string BELOW_LIGHT_COLOR_PROPERTY_NAME = "BelowLightColor";

        private List<PAction> m_ActivationListeners = new List<PAction>();

        private Color m_AmbientLightColor = new Color(0.25f, 0.25f, 0.25f, 1f);
        private Color m_AtmosphereColor = Color.white;
        private float m_GlobalBrightness = 1f;

        private Light m_AboveLight;
        private const float k_AboveLightIntensity = 1f;
        private Light m_BelowLight;
        private const float k_BelowLightIntensity = 0.5f;

        protected override void Awake() {
            base.Awake();

            m_AboveLight = new GameObject("TopLight").AddComponent<Light>();
            m_AboveLight.transform.parent = cachedTransform;
            m_AboveLight.type = LightType.Directional;
            m_AboveLight.shadows = LightShadows.Hard;
            m_AboveLight.transform.localPosition = new Vector3(-5f, 10f, 5f);
            m_AboveLight.transform.LookAt(Vector3.zero);
            m_AboveLight.intensity = k_AboveLightIntensity;

            m_BelowLight = new GameObject("BottomLight").AddComponent<Light>();
            m_BelowLight.transform.parent = cachedTransform;
            m_BelowLight.type = LightType.Directional;
            m_BelowLight.transform.localPosition = new Vector3(5f, -10f, -5f);
            m_BelowLight.transform.LookAt(Vector3.zero);
            m_BelowLight.intensity = k_BelowLightIntensity;

            RegisterPropertyDelegate(FOG_DENSITY_PROPERTY_NAME, OnUpdateFogDensity);
            RegisterPropertyDelegate(ATMOSPHERE_COLOR_PROPERTY_NAME, OnUpdateAtmosphereColor);
            RegisterPropertyDelegate(GLOBAL_BRIGHTNESS_PROPERTY_NAME, OnUpdateGlobalBrightness);
            RegisterPropertyDelegate(AMBIENT_LIGHT_COLOR_PROPERTY_NAME, OnUpdateAmbientLightColor);
            RegisterPropertyDelegate(ABOVE_LIGHT_COLOR_PROPERTY_NAME, OnUpdateAboveLightColor);
            RegisterPropertyDelegate(BELOW_LIGHT_COLOR_PROPERTY_NAME, OnUpdateBelowLightColor);

        }

        public void AddActivationListener(PAction inListener) {
            m_ActivationListeners.Add(inListener);
        }

        public void Activate() {
            for (int i = 0, count = m_ActivationListeners.Count; i < count; ++i) {
                m_ActivationListeners[i].Call();
            }
        }

        private void OnUpdateFogDensity(TValue inValue) {
            var density = (float)inValue.RawStruct<Primitives.Portion>().Value;
            RenderSettings.fogDensity = density;
            RenderSettings.fog = density > float.Epsilon;
        }

        private void OnUpdateAtmosphereColor(TValue inValue) {
            var color = inValue.RawObject<Primitives.Color>().Value;
            m_AtmosphereColor = new Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
            
            UpdateAtmosphereColor();
        }

        private void OnUpdateGlobalBrightness(TValue inValue) {
            var brightness = (float)inValue.RawStruct<Primitives.Portion>().Value;
            m_GlobalBrightness = brightness;
            //RenderSettings.ambientIntensity = brightness;
            RenderSettings.reflectionIntensity = brightness;
            m_AboveLight.intensity = k_AboveLightIntensity * brightness;
            m_BelowLight.intensity = k_BelowLightIntensity * brightness;

            UpdateAtmosphereColor();
            UpdateAmbientLightColor();
        }

        private void OnUpdateAmbientLightColor(TValue inValue) {
            var color = inValue.RawObject<Primitives.Color>().Value;
            m_AmbientLightColor = new Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
            UpdateAmbientLightColor();
        }

        private void OnUpdateAboveLightColor(TValue inValue) {
            var color = inValue.RawObject<Primitives.Color>().Value;
            m_AboveLight.color = new Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
        }

        private void OnUpdateBelowLightColor(TValue inValue) {
            var color = inValue.RawObject<Primitives.Color>().Value;
            m_BelowLight.color = new Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
        }

        private void UpdateAtmosphereColor() {
            Camera.main.backgroundColor = RenderSettings.fogColor = Color.Lerp(Color.clear, 
                                                                               m_AtmosphereColor, 
                                                                               m_GlobalBrightness);
        }   

        private void UpdateAmbientLightColor() {
            RenderSettings.ambientLight = Color.Lerp(new Color(0,0,0,1), 
                                                     m_AmbientLightColor, 
                                                     m_GlobalBrightness);
        }

        public override void CleanUp() {

        }

    }
}