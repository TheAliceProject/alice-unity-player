using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGDisc : SGShape {

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.DiscMesh; } }
        
        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(RADIUS_PROPERTY_NAME, OnRadiusPropertyChanged);
        }

        private void OnRadiusPropertyChanged(TValue inValue) {
            float radius = (float)inValue.ToDouble();

            // keep width of size synced with radius
            var size = this.GetSize(false);
            float w = radius*2;
            float d = (size.z/size.x)*w;

            SetSize(new UnityEngine.Vector3(w, size.y, d));
        }
    }
}