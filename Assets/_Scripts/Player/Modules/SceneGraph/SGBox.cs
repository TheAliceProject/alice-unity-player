using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Modules {
    [PInteropType]
    public sealed class SGBox : SGEntity {

        private Transform m_BoxTransform;

        #region Interop Interfaces
        
        #endregion //Interop Interafaces

        private void Awake() {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_BoxTransform = go.transform;
            m_BoxTransform.SetParent(transform, false);
            m_BoxTransform.localPosition = UnityEngine.Vector3.zero;
            m_BoxTransform.localRotation = UnityEngine.Quaternion.identity;
            Init(go.GetComponent<MeshRenderer>());
        }

        protected override void Init(Renderer inRenderer) {
            base.Init(inRenderer);
            RegisterPropertyCallback<Size>(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
        }

        private void OnSizePropertyChanged(PropertyBase<Size> inProperty) {
            m_BoxTransform.localScale = inProperty.getValue();
        }

    }
}