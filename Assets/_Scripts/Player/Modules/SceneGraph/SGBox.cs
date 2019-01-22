using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGBox : SGShape {
        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.BoxMesh; } }

        protected override void SetSize(UnityEngine.Vector3 size) {
            base.SetSize(size);
            m_ModelTransform.localPosition = new UnityEngine.Vector3(0,size.y*0.5f, 0);
        }
    }
}
