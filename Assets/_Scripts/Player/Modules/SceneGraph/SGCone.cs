using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGCone : SGShape {
        protected override void Awake() {
            base.Awake();

            var go = new GameObject("Model");
            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = SceneGraph.Current.InternalResources.ConeMesh;
            var rend = go.AddComponent<MeshRenderer>();
            
            Init(go.transform, rend);
        }

    }
}