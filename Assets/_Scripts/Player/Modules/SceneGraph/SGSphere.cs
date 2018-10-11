using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGSphere : SGShape {
        public const string RADIUS_PROPERTY_NAME = "Radius";

        protected override void Awake() {
            base.Awake();
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var t = go.transform;
            t.SetParent(transform, false);
            t.localPosition = UnityEngine.Vector3.zero;
            t.localRotation = UnityEngine.Quaternion.identity;
            Init(t, go.GetComponent<MeshRenderer>());
        }

        protected override void Init(Transform inModelTransform, Renderer inRenderer) {
            base.Init(inModelTransform, inRenderer);
            RegisterPropertyDelegate(RADIUS_PROPERTY_NAME, OnRadiusPropertyChanged);
        }

        private void OnRadiusPropertyChanged(TValue inValue) {

            float scale = ((float)inValue.ToDouble())/0.5f;
            m_ModelTransform.localScale = new UnityEngine.Vector3(scale, scale, scale);
        }

        protected override void OnSizePropertyChanged(TValue inValue) {

        }

    }
}