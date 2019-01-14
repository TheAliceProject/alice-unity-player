using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGBillboard : SGShape {

        private MaterialPropertyBlock m_BackPropertyBlock;
        private Paint m_BackCachedPaint = Primitives.Color.WHITE;
        private Renderer m_BackRenderer;

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.BillboardMesh; } }

        protected override void Init(Transform inModelTransform, Renderer inRenderer, MeshFilter inFilter) {
            Transform  backTransform;
            MeshFilter  backFilter;

            CreateModelObject(ShapeMesh, OpaqueMaterial, inModelTransform, out backTransform, out m_BackRenderer, out backFilter);

            base.Init(inModelTransform, inRenderer, inFilter);

            RegisterPropertyDelegate(BACK_PAINT_PROPERTY_NAME, OnBackPaintPropertyChanged);
        }

        private void OnBackPaintPropertyChanged(TValue inValue) {

        }
    }
}
