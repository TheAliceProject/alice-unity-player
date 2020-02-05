using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class SGCylinder : SGShape {
        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.CylinderMesh; } }

        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(RADIUS_PROPERTY_NAME, OnRadiusPropertyChanged);
            RegisterPropertyDelegate(LENGTH_PROPERTY_NAME, OnLengthPropertyChanged);
        }

        private void OnRadiusPropertyChanged(TValue inValue) {
            float radius = (float)inValue.ToDouble();

            // keep width of size synced with radius
            var size = this.GetSize(false);
            float w = radius*2;
            float d = (size.z/size.x)*w;

            SetSize(new UnityEngine.Vector3(w, size.y, d));
        }

        private void OnLengthPropertyChanged(TValue inValue) {
            // keep width of size synced with radius
            var size = this.GetSize(false);
            size.y = (float)inValue.ToDouble();

            SetSize(size);
        }

        protected override void SetSize(UnityEngine.Vector3 size) {
            base.SetSize(size);
            m_ModelTransform.localPosition = new UnityEngine.Vector3(0,size.y*0.5f, 0);
        }
    }
}