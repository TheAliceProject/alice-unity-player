using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGSphere : SGShape {
        public const string RADIUS_PROPERTY_NAME = "Radius";

        private float m_Radius = 0.5f;

        protected override void Awake() {
            base.Awake();
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Init(go.transform, go.GetComponent<MeshRenderer>());
        }

        /* 
        protected override void Init(Transform inModelTransform, Renderer inRenderer) {
            base.Init(inModelTransform, inRenderer);
            RegisterPropertyDelegate(RADIUS_PROPERTY_NAME, OnRadiusPropertyChanged);
        }

        private void OnRadiusPropertyChanged(TValue inValue) {
            // keep x coord of scale synced with radius
            m_Radius = (float)inValue.ToDouble();
            float xScale = m_Radius*2f;
            float yScale = (m_ModelTransform.localScale.y/m_ModelTransform.localScale.x)*xScale;
            float zScale = (m_ModelTransform.localScale.z/m_ModelTransform.localScale.x)*xScale;

            m_ModelTransform.localScale = new UnityEngine.Vector3(xScale, yScale, zScale);
        }

        protected override void SetSize(Size size) {
            // keep x coord of scale synced with radius
            m_Radius = (float)size.Value.X*0.5f;
            base.SetSize(size);
        }*/

    }
}