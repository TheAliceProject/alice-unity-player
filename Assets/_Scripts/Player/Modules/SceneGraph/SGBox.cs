using UnityEngine;

namespace Alice.Player.Unity {
    public sealed class SGBox : SGShape {
        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.BoxMesh; } }
    }
}
