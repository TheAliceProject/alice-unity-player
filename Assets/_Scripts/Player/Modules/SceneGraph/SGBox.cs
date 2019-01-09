using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGBox : SGShape {

        protected override Mesh ShapeMesh { get { return SceneGraph.Current.InternalResources.BoxMesh; } }
    }
}
