using UnityEngine;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    public abstract class ResourceCache<T> where T : UnityEngine.Object {
        private Dictionary<string, T> m_Cache;

        public ResourceCache() {
            m_Cache = new Dictionary<string, T>();
        }

        public virtual bool Add(string inIdentifier, T inTexture) {
            if (m_Cache.ContainsKey(inIdentifier)) {
                Debug.LogWarningFormat("Cache already has resource for identifier {0}.", inIdentifier);
                return false;
            }
            
            m_Cache.Add(inIdentifier, inTexture);
            return true;
        }

        public virtual T Get(string inIdentifier) {
            T resource;

            if (m_Cache.TryGetValue(inIdentifier, out resource)) {
                return resource;
            }

            throw new SceneGraphException(string.Format("No resources with identifier {0} found.", inIdentifier));
        }

        public virtual void Clear() {
            foreach (var resource in m_Cache.Values) {
                UnityEngine.Object.Destroy(resource);
            }
            m_Cache.Clear();
        }

    }


}