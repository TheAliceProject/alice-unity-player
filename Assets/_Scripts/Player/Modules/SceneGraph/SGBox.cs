using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGBox : SGShape {

        protected override void Awake() {
            base.Awake();
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var t = go.transform;
            t.SetParent(transform, false);
            t.localPosition = UnityEngine.Vector3.zero;
            t.localRotation = UnityEngine.Quaternion.identity;
            Init(t, go.GetComponent<MeshRenderer>());
        }

        protected override void OnSizePropertyChanged(TValue inValue) {
            var size = inValue.RawObject<Size>();
            m_ModelTransform.localScale = size;
        }  
    }
}