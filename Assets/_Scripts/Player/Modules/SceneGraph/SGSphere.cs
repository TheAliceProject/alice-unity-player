using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Alice.Player.Unity {
    public sealed class SGSphere : SGShape {

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.SphereMesh; } }
        
        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(RADIUS_PROPERTY_NAME, OnRadiusPropertyChanged);
            m_ModelTransform.rotation = Quaternion.AngleAxis(90, Vector3.up);
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