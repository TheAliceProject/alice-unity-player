using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Modules {
    public sealed class SGSphere : SGModel {
        public const string RADIUS_PROPERTY_NAME = "Radius";

        private Transform m_SphereTransform;

        


        private void Awake() {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_SphereTransform = go.transform;
            m_SphereTransform.SetParent(transform, false);
            m_SphereTransform.localPosition = UnityEngine.Vector3.zero;
            m_SphereTransform.localRotation = UnityEngine.Quaternion.identity;
            Init(go.GetComponent<MeshRenderer>());
        }

        protected override void Init(Renderer inRenderer) {
            base.Init(inRenderer);
            RegisterPropertyDelegate<double>(RADIUS_PROPERTY_NAME, OnRadiusPropertyChanged);
        }

        private void OnRadiusPropertyChanged(TValue inValue) {

            float scale = ((float)inValue.ToDouble())/0.5f;
            m_SphereTransform.localScale = new UnityEngine.Vector3(scale, scale, scale);
        }

        protected override void OnSizePropertyChanged(TValue inValue) {

        }

    }
}