using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGSphere : SGShape {

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.SphereMesh; } }
        
        protected override void Init(Transform inModelTransform, Renderer inRenderer, MeshFilter inFilter) {
            base.Init(inModelTransform, inRenderer, inFilter);
            RegisterPropertyDelegate(RADIUS_PROPERTY_NAME, OnRadiusPropertyChanged);
        }

        private void OnRadiusPropertyChanged(TValue inValue) {
            float radius = (float)inValue.ToDouble();

            // keep width of size synced with radius
            var size = this.GetSize(false);
            float w = radius*2;
            float h = (size.y/size.x)*w;
            float d = (size.z/size.x)*w;

            SetSize(new UnityEngine.Vector3(w, h, d));
        }
    }
}