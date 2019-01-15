using UnityEngine;
using System.Collections.Generic;
using Alice.Tweedle.Parse;
using Alice.Tweedle.File;

namespace Alice.Player.Unity {
    public sealed class TextureCache {

        private TweedleSystem m_System;
        private Dictionary<string, Texture2D> m_Cache;

        public TextureCache(TweedleSystem inSystem) {
            m_System = inSystem;
            m_Cache = new Dictionary<string, Texture2D>();
        }


        Texture2D Get(string inIdentifier) {
            Texture2D texture;

            if (m_Cache.TryGetValue(inIdentifier, out texture)) {
                return texture;
            }

            var id = new ResourceIdentifier(inIdentifier, ContentType.Image, "png");

            ResourceReference resource;
            if (m_System.Resources.TryGetValue(id, out resource)) {
                
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