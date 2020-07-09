using System.Collections;
using UnityEngine;
using Alice.Tweedle.File;

namespace Alice.Tweedle.Parse {
    [CreateAssetMenu(menuName = "Player Library Manifest", fileName = "PlayerLibraryManifest")]
    public sealed class PlayerLibraryManifest : ScriptableObject {

        private static PlayerLibraryManifest m_Instance;
        public static PlayerLibraryManifest Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = Resources.Load<PlayerLibraryManifest>("PlayerLibraryManifest");
                }
                return m_Instance;
            }
        }

        public string GetLibraryVersion()
        {
            // Currently we only use one built in library
            if(m_BuiltInLibraries.Length > 0)
                return m_BuiltInLibraries[0].identifier.version;
            else 
                return "None";
        }

        public string playerVersion = "0.1";
        public string aliceVersion = "3.6.0.0";

        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_Libraries")]
        private PlayerLibraryReference[] m_BuiltInLibraries = null;

        public bool TryGetLibrary(ProjectIdentifier inIdentifier, out PlayerLibraryReference outLibRef) {

            foreach (var libRef in m_BuiltInLibraries) {
                if (libRef.identifier .EqualsVersionMatch(inIdentifier)) {
                    outLibRef = libRef;
                    return true;
                }
            }

            outLibRef = new PlayerLibraryReference();
            return false;
        }

    }

}