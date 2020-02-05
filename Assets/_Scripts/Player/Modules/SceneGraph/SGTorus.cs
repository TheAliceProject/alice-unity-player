using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGTorus : SGShape {

        private const float k_RingRadiusRef = 0.5f;
        private const float k_CrossRadiusRef = 0.25f;

        private float m_InnerRadius = 0.25f;
        private float m_OuterRadius = 0.75f;

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.TorusMesh; } }
        protected override Material OpaqueMaterial { get { return SceneGraph.Current?.InternalResources?.OpaqueTorusMaterial; } }
        protected override Material TransparentMaterial { get { return SceneGraph.Current?.InternalResources?.TransparentTorusMaterial; } }

        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(INNER_RADIUS_PROPERTY_NAME, OnInnerRadiusPropertyChanged);
            RegisterPropertyDelegate(OUTER_RADIUS_PROPERTY_NAME, OnOuterRadiusPropertyChanged);
        }

        private void OnInnerRadiusPropertyChanged(TValue inValue) {
            m_InnerRadius = (float)inValue.ToDouble();
            UpdateRadiusProperties();
        }

        private void OnOuterRadiusPropertyChanged(TValue inValue) {
            m_OuterRadius = (float)inValue.ToDouble();
            UpdateRadiusProperties();
        }

        protected override void SetSize(UnityEngine.Vector3 size) {
            var outerRadius = size.x*0.5f;
            
            if (outerRadius != m_OuterRadius) {
                m_InnerRadius *= (outerRadius/m_OuterRadius);
                m_OuterRadius = outerRadius;
                UpdateRadiusProperties();
            }
            
            base.SetSize(size);
        }

        private void UpdateRadiusProperties() {
            GetPropertyBlock(m_Renderer, ref m_PropertyBlock);
            var height = m_OuterRadius - m_InnerRadius;
            var crossRadius = height*0.5f;
            m_PropertyBlock.SetFloat("_RingRadiusDelta", (m_InnerRadius + crossRadius)-k_RingRadiusRef);
            m_PropertyBlock.SetFloat("_CrossRadiusDelta", crossRadius-k_CrossRadiusRef);
            m_Renderer.SetPropertyBlock(m_PropertyBlock);

            var diam = m_OuterRadius*2f;

            m_MeshFilter.mesh.bounds = new Bounds(UnityEngine.Vector3.zero, 
                                                  new UnityEngine.Vector3(diam, height, diam));
            CacheMeshBounds();
        }

        private void OnDrawGizmosSelected() {
            Gizmos.matrix = m_ModelTransform.localToWorldMatrix;
            Gizmos.DrawWireCube(UnityEngine.Vector3.zero, MeshBounds.size);
        }
    }
}