using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {

        protected override Material OpaqueMaterial { get { return m_Renderer?.sharedMaterial; } }
        protected override Material TransparentMaterial { get { return m_Renderer?.sharedMaterial; } }

        public void SetResource(string inIdentifier) {
            var prefab = SceneGraph.Current.ModelCache.Get(inIdentifier);

            if (prefab) {
                var model = Instantiate(prefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, cachedTransform);

                m_ModelTransform = model.transform;
                //TODO: bounds and size should use max aabb of all renderers
                m_MeshFilter = model.GetComponentInChildren<MeshFilter>();
                m_Renderer = model.GetComponentInChildren<Renderer>();
                CacheMeshBounds();
            }
        }
    }
}