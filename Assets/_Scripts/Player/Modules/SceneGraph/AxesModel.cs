using UnityEngine;

namespace Alice.Player.Unity {
    public sealed class AxesModel : MonoBehaviour {
        [SerializeField]
        private Renderer m_ForwardCylinderRenderer = null;
        public Renderer ForwardCylinderRenderer { get { return m_ForwardCylinderRenderer; } }

        [SerializeField]
        private Renderer m_ForwardConeRenderer = null;
        public Renderer ForwardConeRenderer { get { return m_ForwardConeRenderer; } }

        [SerializeField]
        private Renderer m_BackCylinderRenderer = null;
        public Renderer BackCylinderRenderer { get { return m_BackCylinderRenderer; } }

        [SerializeField]
        private Renderer m_BackConeRenderer = null;
        public Renderer BackConeRenderer { get { return m_BackConeRenderer; } }


        [SerializeField]
        private Renderer m_RightCylinderRenderer = null;
        public Renderer RightCylinderRenderer { get { return m_RightCylinderRenderer; } }

        [SerializeField]
        private Renderer m_RightConeRenderer = null;
        public Renderer RightConeRenderer { get { return m_RightConeRenderer; } }

        [SerializeField]
        private Renderer m_UpCylinderRenderer = null;
        public Renderer UpCylinderRenderer { get { return m_UpCylinderRenderer; } }

        [SerializeField]
        private Renderer m_UpConeRenderer = null;
        public Renderer UpConeRenderer { get { return m_UpConeRenderer; } }

        private Bounds m_MeshBounds = new Bounds(new Vector3(0.5f, 0.5f, 0f), new Vector3(1f, 1f, 2f));
        public Bounds bounds { get { return m_MeshBounds; } }
    }
}