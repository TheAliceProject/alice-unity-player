using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {

        private Renderer[] m_Renderers;
        private MeshFilter[] m_Filters;
        private MaterialPropertyBlock[] m_PropertyBlocks;

        private int m_BoundsRendererIndex = -1;

        public void SetResource(string inIdentifier) {
            var prefab = SceneGraph.Current.ModelCache.Get(inIdentifier);

            if (prefab) {
                var model = Instantiate(prefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, cachedTransform);

                m_ModelTransform = model.transform;

                m_Filters = model.GetComponentsInChildren<MeshFilter>();
                if (m_Renderers == null) {
                    m_PropertyBlocks = new MaterialPropertyBlock[m_Filters.Length];
                    m_Renderers = new Renderer[m_Filters.Length];
                } else if (m_Renderers.Length != m_Filters.Length) {
                    System.Array.Resize(ref m_PropertyBlocks, m_Filters.Length);
                    System.Array.Resize(ref m_Renderers, m_Filters.Length);
                }

                float largetVolume = 0;
                m_BoundsRendererIndex = -1;

                for (int i = 0; i < m_Renderers.Length; ++i) {
                    m_Renderers[i] = m_Filters[i].GetComponent<Renderer>();

                    PrepPropertyBlock(m_Renderers[i], ref m_PropertyBlocks[i]);
                    m_PropertyBlocks[i].SetTexture(MAIN_TEXTURE_SHADER_NAME, m_Renderers[i].sharedMaterial.mainTexture);
                    
                    
                    UnityEngine.Vector3 size;
                    if (m_Renderers[i] is SkinnedMeshRenderer) {
                        // make sure the skinned mesh renderers local bounds get updated
                        var skinnedRenderer = (SkinnedMeshRenderer)m_Renderers[i];
                        skinnedRenderer.updateWhenOffscreen = true;
                        size = skinnedRenderer.localBounds.size;
                    } else {
                        size = m_Filters[i].sharedMesh.bounds.size;
                    }

                    var volume = size.x*size.y*size.z;

                    if (volume > largetVolume) {
                        largetVolume = volume;
                        m_BoundsRendererIndex = i;
                    }
                }
                CacheMeshBounds();
            }
        }

        protected override Bounds GetMeshBounds() {
            if (m_Renderers[m_BoundsRendererIndex] is SkinnedMeshRenderer) {
                var skinnedRenderer = (SkinnedMeshRenderer)m_Renderers[m_BoundsRendererIndex];
                return skinnedRenderer.localBounds;
            }

            return m_Filters[m_BoundsRendererIndex].sharedMesh.bounds;
        }

        protected override void OnPaintChanged() {
            for (int i = 0; i < m_Renderers.Length; ++i) {
                ApplyPaint(m_Renderers[i], ref m_PropertyBlocks[i]);
            }
        }

        protected override void OnOpacityChanged() {
            for (int i = 0; i < m_Renderers.Length; ++i) {
                ApplyOpacity(m_Renderers[i], ref m_PropertyBlocks[i]);
            }
        }
    }
}