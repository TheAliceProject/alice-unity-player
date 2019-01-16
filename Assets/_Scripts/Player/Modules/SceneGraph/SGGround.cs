using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGGround : SGShape {
        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.GroundMesh; } }

        protected override void Init(Transform inModelTransform, Renderer inRenderer, MeshFilter inFilter) {
            base.Init(inModelTransform, inRenderer, inFilter);

            // plane mesh is 10x10
            inModelTransform.localScale = new UnityEngine.Vector3(100, 100, 100);
            inModelTransform.localPosition = new UnityEngine.Vector3(0,-0.0001f,0);
        }
    }
}