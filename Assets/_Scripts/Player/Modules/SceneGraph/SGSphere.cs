using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Modules {
    [PInteropType]
    public sealed class SGSphere : SGEntity {

        private Transform m_SphereTransform;

        #region Interop Interfaces
        [PInteropField]
        public const string RADIUS_PROPERTY_NAME = "Radius";

        #endregion //Interop Interafaces

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

        private void OnRadiusPropertyChanged(PropertyBase<double> inProperty) {
            float scale = ((float)inProperty.getValue())/0.5f;
            m_SphereTransform.localScale = new UnityEngine.Vector3(scale, scale, scale);
        }

    }
}