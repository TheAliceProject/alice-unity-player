using UnityEngine;


namespace Alice.Player.Unity {
    public sealed class SGAxes : SGModel {
        
        private static readonly Color k_ForwardColor = Color.white;
        private static readonly Color k_BackColor = Color.blue;
        private static readonly Color k_RightColor = Color.red;
        private static readonly Color k_UpColor = Color.green;

        
        AxesModel m_Model;

        MaterialPropertyBlock[] m_PropertyBlocks = new MaterialPropertyBlock[8]; 

        protected override void Awake() {
            base.Awake();

            m_Model = Instantiate(SceneGraph.Current.InternalResources.AxesModel, cachedTransform, false);
            m_ModelTransform = m_Model.transform;
            m_ModelTransform.localPosition = Vector3.zero;
            m_ModelTransform.localRotation = Quaternion.identity;
            
            

            CacheMeshBounds();
        }

        private void SetRendererColor(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, Color inColor) {
            PrepPropertyBlock(inRenderer, ref ioPropertyBlock);
            ioPropertyBlock.SetColor("_Color", inColor);
            inRenderer.SetPropertyBlock(ioPropertyBlock);
        }

        protected override Bounds GetMeshBounds() {
            return m_Model.bounds;
        }

        protected override void OnPaintChanged() {
            SetRendererColor(m_Model.ForwardCylinderRenderer, ref m_PropertyBlocks[0], k_ForwardColor);
            SetRendererColor(m_Model.ForwardConeRenderer, ref m_PropertyBlocks[1], k_ForwardColor);

            SetRendererColor(m_Model.BackCylinderRenderer, ref m_PropertyBlocks[2], k_BackColor);
            SetRendererColor(m_Model.BackConeRenderer, ref m_PropertyBlocks[3], k_BackColor);

            SetRendererColor(m_Model.RightCylinderRenderer, ref m_PropertyBlocks[4], k_RightColor);
            SetRendererColor(m_Model.RightConeRenderer, ref m_PropertyBlocks[5], k_RightColor);

            SetRendererColor(m_Model.UpCylinderRenderer, ref m_PropertyBlocks[6], k_UpColor);
            SetRendererColor(m_Model.UpConeRenderer, ref m_PropertyBlocks[7], k_UpColor);
        }

        protected override void OnOpacityChanged() {
            ApplyOpacity(m_Model.ForwardCylinderRenderer, ref m_PropertyBlocks[0]);
            ApplyOpacity(m_Model.ForwardConeRenderer, ref m_PropertyBlocks[1]);

            ApplyOpacity(m_Model.BackCylinderRenderer, ref m_PropertyBlocks[2]);
            ApplyOpacity(m_Model.BackConeRenderer, ref m_PropertyBlocks[3]);

            ApplyOpacity(m_Model.RightCylinderRenderer, ref m_PropertyBlocks[4]);
            ApplyOpacity(m_Model.RightConeRenderer, ref m_PropertyBlocks[5]);

            ApplyOpacity(m_Model.UpCylinderRenderer, ref m_PropertyBlocks[6]);
            ApplyOpacity(m_Model.UpConeRenderer, ref m_PropertyBlocks[7]);
        }

    }
}