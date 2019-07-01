using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;
using BeauRoutine;
using System.Collections;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {
        
        private string m_ResourceId;
        private Renderer[] m_Renderers;
        private MeshFilter[] m_Filters;
        private MaterialPropertyBlock[] m_PropertyBlocks;

        private int m_BoundsRendererIndex = -1;

        public void SetResource(string inIdentifier) {

            if (m_ResourceId == inIdentifier) {
                return;
            }

            m_ResourceId = inIdentifier;

            if (m_ModelTransform) {
                Destroy(m_ModelTransform.gameObject);
                m_ModelTransform = null;
            }

            var prefab = SceneGraph.Current.ModelCache.Get(inIdentifier);
            Texture myTexture = SceneGraph.Current.ModelTextureCache[inIdentifier];

            if (prefab) {
                var model = Instantiate(prefab, cachedTransform, false);
                m_ModelTransform = model.transform;
                m_ModelTransform.localRotation = UnityEngine.Quaternion.identity;
                m_ModelTransform.localPosition = UnityEngine.Vector3.zero;

                m_Filters = model.GetComponentsInChildren<MeshFilter>();
                if (m_Renderers == null) {
                    m_PropertyBlocks = new MaterialPropertyBlock[m_Filters.Length];
                    m_Renderers = new Renderer[m_Filters.Length];
                } else if (m_Renderers.Length != m_Filters.Length) {
                    System.Array.Resize(ref m_PropertyBlocks, m_Filters.Length);
                    System.Array.Resize(ref m_Renderers, m_Filters.Length);
                }

                float largestVolume = -1;
                m_BoundsRendererIndex = -1;
                
                for (int i = 0; i < m_Renderers.Length; ++i) {
                    m_Renderers[i] = m_Filters[i].GetComponent<Renderer>();


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

                    if (volume > largestVolume) {
                        largestVolume = volume;
                        m_BoundsRendererIndex = i;
                    }
                    ApplyCurrentPaintAndOpacity(m_Renderers[i], ref m_PropertyBlocks[i]);
                }
                CacheMeshBounds();
            } else {
                m_Renderers = null;
                m_Filters = null;
            }
        }

        protected override Bounds GetMeshBounds() {
            if (m_Filters == null) {
                return new Bounds(UnityEngine.Vector3.zero, UnityEngine.Vector3.zero);
            }

            if (m_Renderers[m_BoundsRendererIndex] is SkinnedMeshRenderer) {
                var skinnedRenderer = (SkinnedMeshRenderer)m_Renderers[m_BoundsRendererIndex];
                return skinnedRenderer.localBounds;
            }

            return m_Filters[m_BoundsRendererIndex].sharedMesh.bounds;
        }

        protected override void OnPaintChanged() {
            for (int i = 0; i < m_Renderers?.Length; ++i) {
                //ApplyPaint(m_Renderers[i], ref m_PropertyBlocks[i]);
            }
        }

        protected override void OnOpacityChanged() {
            for (int i = 0; i < m_Renderers?.Length; ++i) {
                ApplyOpacity(m_Renderers[i], ref m_PropertyBlocks[i]);
            }
        }

        public SGJoint LinkJoint(TValue inOwner, string inName) {
            var bone = FindInHierarchy(m_ModelTransform, inName.ToUpper());
            SGJoint joint = null;
            if (bone != null) {
                joint = SGEntity.Create<SGJoint>(inOwner, bone);
            }
            return joint;
        }

        private GameObject FindInHierarchy(Transform inTransform, string inName) {
            

            foreach (Transform child in inTransform) {
                if (child.gameObject.name == inName) {
                    return child.gameObject;
                }

                var match = FindInHierarchy(child, inName);
                if (match) {
                    return match;
                }
            }
            
            return null;
        }
    }
}