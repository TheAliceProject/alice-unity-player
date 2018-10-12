using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public sealed class SGScene : SGEntity {

        public const string FOG_DENSITY_PROPERTY_NAME = "Fog";
        public const string GLOBAL_BRIGHTNESS_PROPERTY_NAME = "Brightness";

        private List<PAction> m_ActivationListeners = new List<PAction>();

        private bool m_BackgroundColorCached = false;
        private UnityEngine.Color m_BackgroundColor;

        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(FOG_DENSITY_PROPERTY_NAME, OnUpdateFogDensity);
            RegisterPropertyDelegate(GLOBAL_BRIGHTNESS_PROPERTY_NAME, OnUpdateGlobalBrightness);
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
            var density = (float)inValue.RawObject<Portion>().Value;
            RenderSettings.fogDensity = density;
            RenderSettings.fog = density > float.Epsilon;
            UpdateBackgroundColor();
        }

        private void OnUpdateGlobalBrightness(TValue inValue) {
            var brightness = (float)inValue.RawObject<Portion>().Value;
            RenderSettings.ambientIntensity = brightness;
            RenderSettings.reflectionIntensity = brightness;

           UpdateBackgroundColor();
        }

        private void UpdateBackgroundColor() {
             if (!m_BackgroundColorCached) {
                m_BackgroundColorCached = true;
                m_BackgroundColor = Camera.main.backgroundColor;
            }

            Camera.main.backgroundColor = UnityEngine.Color.Lerp(UnityEngine.Color.clear, 
                                                                 UnityEngine.Color.Lerp(m_BackgroundColor,
                                                                                        RenderSettings.fogColor,
                                                                                        RenderSettings.fogDensity), 
                                                                 RenderSettings.ambientIntensity);
        }

    }
}