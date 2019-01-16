using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGCamera : SGTransformableEntity {
        public Camera Camera {get; private set;}

        protected override void Awake() {
            base.Awake();
            Camera = Camera.main;
            Camera.transform.SetParent(cachedTransform, false);
            Camera.transform.localPosition = UnityEngine.Vector3.zero;
            Camera.transform.localRotation = UnityEngine.Quaternion.identity;
        }

        public override void CleanUp() {
            Camera.transform.SetParent(null, true);
            Camera = null;
        }
    }
}