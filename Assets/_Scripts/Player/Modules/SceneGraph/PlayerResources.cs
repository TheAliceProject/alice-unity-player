using UnityEngine;
using TriLib;

namespace Alice.Player.Unity {
    [CreateAssetMenu(menuName = "Player Resources", fileName = "New Player Resources")]
    public sealed class PlayerResources : ScriptableObject {
        [Header("Materials")]
        [SerializeField]
        private Material m_OpaqueMaterial = null;
        public Material OpaqueMaterial { get {return m_OpaqueMaterial; } }

        [SerializeField]
        private Material m_TransparentMaterial = null;
        public Material TransparentMaterial {get { return m_TransparentMaterial; } }

        [SerializeField]
        private Material m_TransparentMultipassMaterial = null;
        public Material TransparentMultipassMaterial {get { return m_TransparentMultipassMaterial; } }

        [SerializeField]
        private Material m_OpaqueTorusMaterial = null;
        public Material OpaqueTorusMaterial { get {return m_OpaqueTorusMaterial; } }

        [SerializeField]
        private Material m_TransparentTorusMaterial = null;
        public Material TransparentTorusMaterial {get { return m_TransparentTorusMaterial; } }

        [Header("Primitive Meshes")]
        [SerializeField]
        private Mesh m_BoxMesh = null;
        public Mesh BoxMesh { get { return m_BoxMesh; } }

        [SerializeField]
        private Mesh m_SphereMesh = null;
        public Mesh SphereMesh { get { return m_SphereMesh; } }

        [SerializeField]
        private Mesh m_CylinderMesh = null;
        public Mesh CylinderMesh { get { return m_CylinderMesh; } }

        [SerializeField]
        private Mesh m_ConeMesh = null;
        public Mesh ConeMesh { get { return m_ConeMesh; } }

        [SerializeField]
        private Mesh m_TorusMesh = null;
        public Mesh TorusMesh { get { return m_TorusMesh; } }

        [SerializeField]
        private Mesh m_BillboardMesh = null;
        public Mesh BillboardMesh { get { return m_BillboardMesh; } }

        [SerializeField]
        private Mesh m_DiscMesh = null;
        public Mesh DiscMesh { get { return m_DiscMesh; } }

        [SerializeField]
        private Mesh m_GroundMesh = null;
        public Mesh GroundMesh { get { return m_GroundMesh; } }

        [SerializeField]
        private Mesh m_WallMesh = null;
        public Mesh WallMesh { get { return m_WallMesh; } }

        [SerializeField]
        private AxesModel m_AxesModel = null;
        public AxesModel AxesModel { get { return m_AxesModel; } }

        [SerializeField]
        private GameObject m_FlyingText = null;
        public GameObject FlyingTextObject { get { return m_FlyingText; } }

        [Header("VR")]
        [SerializeField]
        private VRRig m_VRRig = null;
        public VRRig VRRig { get { return m_VRRig;  } }
        
        [Header("UI")]
        [SerializeField]
        private SceneCanvas m_SceneCanvas = null;
        public SceneCanvas SceneCanvas { get { return m_SceneCanvas; } }

        [Header("Audio")]
        [SerializeField]
        private TweedleAudioPlayer m_TweedleAudioSource = null;
        public TweedleAudioPlayer TweedleAudioSource { get { return m_TweedleAudioSource; } }

        [Header("TriLib Loader Options")]
        AssetLoaderOptions m_ModelLoaderOptions;
        public AssetLoaderOptions ModelLoaderOptions {
            get { 
                if (m_ModelLoaderOptions == null) {
                    m_ModelLoaderOptions = AssetLoaderOptions.CreateInstance();
                    m_ModelLoaderOptions.UseOriginalPositionRotationAndScale = true;
                    m_ModelLoaderOptions.DontLoadAnimations = true; 
                    m_ModelLoaderOptions.AutoPlayAnimations = false;
                    m_ModelLoaderOptions.ApplyNormalTexture = true;
                    m_ModelLoaderOptions.MaterialTransparencyMode = MaterialTransparencyMode.Cutout;
                    m_ModelLoaderOptions.PostProcessSteps = AssimpProcessPreset.TargetRealtimeFast | AssimpProcessPreset.ConvertToLeftHanded;
                    m_ModelLoaderOptions.RotationAngles = Vector3.zero;
                }
                return m_ModelLoaderOptions; 
            }
        }

    }


}