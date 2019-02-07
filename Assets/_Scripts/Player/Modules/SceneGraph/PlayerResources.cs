using UnityEngine;
using TriLib;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Alice.Player.Unity {
    public sealed class PlayerResources : ScriptableObject {
        [Header("Materials")]
        [SerializeField]
        private Material m_OpaqueMaterial;
        public Material OpaqueMaterial { get {return m_OpaqueMaterial; } }

        [SerializeField]
        private Material m_TransparentMaterial;
        public Material TransparentMaterial {get { return m_TransparentMaterial; } }

        [SerializeField]
        private Material m_OpaqueTorusMaterial;
        public Material OpaqueTorusMaterial { get {return m_OpaqueTorusMaterial; } }

        [SerializeField]
        private Material m_TransparentTorusMaterial;
        public Material TransparentTorusMaterial {get { return m_TransparentTorusMaterial; } }

        [Header("Primitive Meshes")]
        [SerializeField]
        private Mesh m_BoxMesh;
        public Mesh BoxMesh {get { return m_BoxMesh; } }

        [SerializeField]
        private Mesh m_SphereMesh;
        public Mesh SphereMesh {get { return m_SphereMesh; } }

        [SerializeField]
        private Mesh m_CylinderMesh;
        public Mesh CylinderMesh {get { return m_CylinderMesh; } }

        [SerializeField]
        private Mesh m_ConeMesh;
        public Mesh ConeMesh {get { return m_ConeMesh; } }

        [SerializeField]
        private Mesh m_TorusMesh;
        public Mesh TorusMesh {get { return m_TorusMesh; } }

        [SerializeField]
        private Mesh m_BillboardMesh;
        public Mesh BillboardMesh {get { return m_BillboardMesh; } }

        [SerializeField]
        private Mesh m_DiscMesh;
        public Mesh DiscMesh {get { return m_DiscMesh; } }

        [SerializeField]
        private Mesh m_GroundMesh;
        public Mesh GroundMesh {get { return m_GroundMesh; } }

        [Header("TriLib Loader Options")]
        AssetLoaderOptions m_ModelLoaderOptions;
        public AssetLoaderOptions ModelLoaderOptions {
            get { 
                if (m_ModelLoaderOptions == null) {
                    m_ModelLoaderOptions = AssetLoaderOptions.CreateInstance();
                    m_ModelLoaderOptions.AutoPlayAnimations = false;
                    m_ModelLoaderOptions.PostProcessSteps = AssimpProcessPreset.TargetRealtimeFast;
                    m_ModelLoaderOptions.RotationAngles = Vector3.zero;
                }
                return m_ModelLoaderOptions; 
            }
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Player Resources Asset")]
        static public void CreatePlayerResourcesAsset() {
            var asset =  CreateInstance<PlayerResources>();
            ProjectWindowUtil.CreateAsset(asset, "New" + typeof(PlayerResources).Name + ".asset");
        }
#endif

    }


}