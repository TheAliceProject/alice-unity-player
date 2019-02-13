using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGBillboard : SGShape {

        private MaterialPropertyBlock m_BackPropertyBlock;
        private Paint m_CachedBackPaint = Primitives.Color.WHITE;
        private Renderer m_BackRenderer;

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.BillboardMesh; } }
        // billboards are always transparent
        protected override Material OpaqueMaterial { get { return SceneGraph.Current?.InternalResources?.TransparentMaterial; } }
        protected override Material TransparentMaterial { get { return SceneGraph.Current?.InternalResources?.TransparentMaterial; } }

        protected override void Awake() {
            base.Awake();

            RegisterPropertyDelegate(BACK_PAINT_PROPERTY_NAME, OnBackPaintPropertyChanged);

            Transform  backTransform;
            MeshFilter  backFilter;
            CreateModelObject(ShapeMesh, OpaqueMaterial, m_ModelTransform, out backTransform, out m_BackRenderer, out backFilter);
            backTransform.localRotation = UnityEngine.Quaternion.Euler(0,180,0);
            m_ModelTransform.localRotation = UnityEngine.Quaternion.Euler(0,180,0);
        }

        protected override void SetSize(UnityEngine.Vector3 size) {
            base.SetSize(size);
            m_ModelTransform.localPosition = new UnityEngine.Vector3(0,size.y*0.5f, 0);
        }

        private void OnBackPaintPropertyChanged(TValue inValue) {
            m_CachedBackPaint = inValue.RawObject<Paint>();

            GetPropertyBlock(m_BackRenderer, ref m_BackPropertyBlock);

            m_CachedBackPaint.Apply(m_BackPropertyBlock, m_CachedOpacity, PaintTextureName);
            m_BackRenderer.SetPropertyBlock(m_BackPropertyBlock);
        }

        protected override void OnOpacityChanged() {
            base.OnOpacityChanged();

            if (m_BackRenderer.enabled != m_Renderer.enabled) {
                m_BackRenderer.enabled = m_Renderer.enabled;
            }

            GetPropertyBlock(m_BackRenderer, ref m_BackPropertyBlock);

            m_CachedBackPaint.Apply(m_BackPropertyBlock, m_CachedOpacity, PaintTextureName);
            m_BackRenderer.SetPropertyBlock(m_BackPropertyBlock);
        }
    }
}
