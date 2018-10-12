using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Alice.Player.Unity {
    public sealed class PlayerResources : ScriptableObject {

        [SerializeField]
        private Material m_OpaqueMaterial;
        public Material OpaqueMaterial { get {return m_OpaqueMaterial; } }

        [SerializeField]
        private Material m_TransparentMaterial;
        public Material TransparentMaterial {get { return m_TransparentMaterial; } }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Player Resources Asset")]
        static public void CreatePlayerResourcesAsset() {
            var asset =  CreateInstance<PlayerResources>();
            ProjectWindowUtil.CreateAsset(asset, "New" + typeof(PlayerResources).Name + ".asset");
        }
#endif

    }


}