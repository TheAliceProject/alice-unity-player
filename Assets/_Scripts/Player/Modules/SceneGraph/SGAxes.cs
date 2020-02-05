using UnityEngine;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGAxes : SGModel {
        
        private static readonly Primitives.Color k_ForwardColor = Primitives.Color.WHITE;
        private static readonly Primitives.Color k_BackColor = Primitives.Color.BLUE;
        private static readonly Primitives.Color k_RightColor = Primitives.Color.RED;
        private static readonly Primitives.Color k_UpColor = Primitives.Color.GREEN;
        
        AxesModel m_Model;

        MaterialPropertyBlock[] m_PropertyBlocks = new MaterialPropertyBlock[8]; 

        protected override void Awake() {
            base.Awake();

            m_Model = Instantiate(SceneGraph.Current.InternalResources.AxesModel, cachedTransform, false);
            m_ModelTransform = m_Model.transform;
            m_ModelTransform.localPosition = UnityEngine.Vector3.zero;
            m_ModelTransform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);

            CacheMeshBounds();
        }


        protected override Bounds GetMeshBounds() {
            return m_Model.bounds;
        }

        protected override void OnPaintChanged() {
        }

        protected override void OnOpacityChanged() {
            ApplyPaintAndCurrentOpacity(m_Model.ForwardCylinderRenderer, ref m_PropertyBlocks[0], k_ForwardColor);
            ApplyPaintAndCurrentOpacity(m_Model.ForwardConeRenderer, ref m_PropertyBlocks[1], k_ForwardColor);

            ApplyPaintAndCurrentOpacity(m_Model.BackCylinderRenderer, ref m_PropertyBlocks[2], k_BackColor);
            ApplyPaintAndCurrentOpacity(m_Model.BackConeRenderer, ref m_PropertyBlocks[3], k_BackColor);

            ApplyPaintAndCurrentOpacity(m_Model.RightCylinderRenderer, ref m_PropertyBlocks[4], k_RightColor);
            ApplyPaintAndCurrentOpacity(m_Model.RightConeRenderer, ref m_PropertyBlocks[5], k_RightColor);

            ApplyPaintAndCurrentOpacity(m_Model.UpCylinderRenderer, ref m_PropertyBlocks[6], k_UpColor);
            ApplyPaintAndCurrentOpacity(m_Model.UpConeRenderer, ref m_PropertyBlocks[7], k_UpColor);
        }

    }
}