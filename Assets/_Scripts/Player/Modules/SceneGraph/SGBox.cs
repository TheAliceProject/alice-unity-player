using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Modules {
    public sealed class SGBox : SGShape {

        private Transform m_BoxTransform;

        private void Awake() {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_BoxTransform = go.transform;
            m_BoxTransform.SetParent(transform, false);
            m_BoxTransform.localPosition = UnityEngine.Vector3.zero;
            m_BoxTransform.localRotation = UnityEngine.Quaternion.identity;
            Init(go.GetComponent<MeshRenderer>());
        }

        protected override void OnSizePropertyChanged(TValue inValue) {
            var size = inValue.RawObject<Size>();
            m_BoxTransform.localScale = size;
        }  
    }
}