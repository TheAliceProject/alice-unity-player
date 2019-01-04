using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGBox : SGShape {

        protected override void Awake() {
            base.Awake();
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Init(go.transform, go.GetComponent<MeshRenderer>());
        }
    }
}
