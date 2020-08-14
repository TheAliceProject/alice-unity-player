using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGRoom : SGShape {
        public const string CEILING_PAINT_PROPERTY_NAME = "CeilingPaint";
        public const string FLOOR_PAINT_PROPERTY_NAME = "FloorPaint";
        // Wall Paint is regular paint property

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.WallMesh; } }

        private Renderer m_FloorRenderer;
        private MaterialPropertyBlock m_FloorPropertyBlock;
        private Paint m_FloorPaint = Primitives.Color.WHITE;

        private Renderer m_CeilingRenderer;
        private MaterialPropertyBlock m_CeilingPropertyBlock;
        private Paint m_CeilingPaint = Primitives.Color.WHITE;


        protected override void Awake() {
            base.Awake();

            RegisterPropertyDelegate(CEILING_PAINT_PROPERTY_NAME, OnCeilingPaintChanged);
            RegisterPropertyDelegate(FLOOR_PAINT_PROPERTY_NAME, OnFloorPaintChanged);

            Transform  floorTransform;
            MeshFilter  floorFilter;
            CreateModelObject(SceneGraph.Current?.InternalResources?.GroundMesh, TransparentMaterial, m_ModelTransform, out floorTransform, out m_FloorRenderer, out floorFilter);

            GetPropertyBlock(m_FloorRenderer, ref m_FloorPropertyBlock);
            m_FloorPropertyBlock.SetVector("_MainTex_ST", new Vector4(-10,10,0,0));
            m_FloorRenderer.SetPropertyBlock(m_FloorPropertyBlock);

            Transform  ceilingTransform;
            MeshFilter  ceilingFilter;
            CreateModelObject(SceneGraph.Current?.InternalResources?.GroundMesh, TransparentMaterial, m_ModelTransform, out ceilingTransform, out m_CeilingRenderer, out ceilingFilter);

            GetPropertyBlock(m_CeilingRenderer, ref m_CeilingPropertyBlock);
            m_CeilingPropertyBlock.SetVector("_MainTex_ST", new Vector4(10,10,0,0));
            m_CeilingRenderer.SetPropertyBlock(m_CeilingPropertyBlock);

            ceilingTransform.localPosition = new UnityEngine.Vector3(0,3,0);
            ceilingTransform.eulerAngles = new UnityEngine.Vector3(180,0,0);
        }


        private void OnCeilingPaintChanged(TValue inValue) {
            m_CeilingPaint = inValue.RawObject<Paint>();

            GetPropertyBlock(m_CeilingRenderer, ref m_CeilingPropertyBlock);

            m_CeilingPaint.Apply(m_CeilingPropertyBlock, m_CachedOpacity, PaintTextureName);
            m_CeilingRenderer.SetPropertyBlock(m_CeilingPropertyBlock);
        }

        private void OnFloorPaintChanged(TValue inValue) {
            m_FloorPaint = inValue.RawObject<Paint>();

            GetPropertyBlock(m_FloorRenderer, ref m_FloorPropertyBlock);

            m_FloorPaint.Apply(m_FloorPropertyBlock, m_CachedOpacity, PaintTextureName);
            m_FloorRenderer.SetPropertyBlock(m_FloorPropertyBlock);
        }

        protected override void OnOpacityChanged() {
            base.OnOpacityChanged();

            if (m_CeilingRenderer.enabled != m_Renderer.enabled) {
                m_CeilingRenderer.enabled = m_Renderer.enabled;
            }

            GetPropertyBlock(m_CeilingRenderer, ref m_CeilingPropertyBlock);

            m_CeilingPaint.Apply(m_CeilingPropertyBlock, m_CachedOpacity, PaintTextureName);
            m_CeilingRenderer.SetPropertyBlock(m_CeilingPropertyBlock);

            if (m_FloorRenderer.enabled != m_Renderer.enabled) {
                m_FloorRenderer.enabled = m_Renderer.enabled;
            }

            GetPropertyBlock(m_FloorRenderer, ref m_FloorPropertyBlock);

            m_FloorPaint.Apply(m_FloorPropertyBlock, m_CachedOpacity, PaintTextureName);
            m_FloorRenderer.SetPropertyBlock(m_FloorPropertyBlock);
        }
    }
}