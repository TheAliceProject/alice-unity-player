using UnityEngine;
using System.Collections.Generic;
using Alice.Tweedle.Parse;
using Alice.Tweedle.File;

namespace Alice.Player.Unity {
    public sealed class TextureCache {
        private Dictionary<string, Texture2D> m_Cache;

        public TextureCache() {
            m_Cache = new Dictionary<string, Texture2D>();
        }

        public bool Add(string inIdentifier, Texture2D inTexture) {
            if (m_Cache.ContainsKey(inIdentifier)) {
                Debug.LogWarningFormat("Texture cache already has texture for identifier {0}.", inIdentifier);
                return false;
            }
            
            m_Cache.Add(inIdentifier, inTexture);
            return true;
        }

        public Texture2D Get(string inIdentifier) {
            Texture2D texture;

            if (m_Cache.TryGetValue(inIdentifier, out texture)) {
                return texture;
            }

            throw new SceneGraphException(string.Format("No texture resources with identifier {0} found.", inIdentifier));
        }

        public void Clear() {
            foreach (var tex in m_Cache.Values) {
                Texture2D.Destroy(tex);
            }
            m_Cache.Clear();
        }

    }


}