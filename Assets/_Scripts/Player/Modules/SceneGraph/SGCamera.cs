using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGCamera : SGTransformableEntity {
        private Camera m_Camera;

        protected override void Awake() {
            base.Awake();
            m_Camera = Camera.main;
            m_Camera.transform.SetParent(cachedTransform, false);
            m_Camera.transform.localPosition = UnityEngine.Vector3.zero;
            m_Camera.transform.localRotation = UnityEngine.Quaternion.identity;
        }

        public override void CleanUp() {
            m_Camera.transform.SetParent(null, true);
            m_Camera = null;
        }
    }
}