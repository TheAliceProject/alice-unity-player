using UnityEngine;
using Alice.Tweedle;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {
        
        private string m_ResourceId;
        private Renderer[] m_Renderers;
        private MaterialPropertyBlock[] m_PropertyBlocks;

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

            if (prefab) {
                var model = Instantiate(prefab, cachedTransform, false);
                m_ModelTransform = model.transform;
                m_ModelTransform.localRotation = UnityEngine.Quaternion.identity;
                m_ModelTransform.localPosition = UnityEngine.Vector3.zero;

                m_Renderers = model.GetComponentsInChildren<Renderer>();
                m_PropertyBlocks = new MaterialPropertyBlock[m_Renderers.Length];

                for (int i = 0; i < m_Renderers.Length; ++i) {
                    GetPropertyBlock(m_Renderers[i], ref m_PropertyBlocks[i]);
                    m_PropertyBlocks[i].SetTexture(MAIN_TEXTURE_SHADER_NAME, m_Renderers[i].sharedMaterial.mainTexture);

                    if (m_Renderers[i] is SkinnedMeshRenderer) {
                        // make sure the skinned mesh renderers local bounds get updated
                        var skinnedRenderer = (SkinnedMeshRenderer)m_Renderers[i];
                        skinnedRenderer.updateWhenOffscreen = true;
                    }
                    ApplyCurrentPaintAndOpacity(m_Renderers[i], ref m_PropertyBlocks[i]);
                }
                CacheMeshBounds();
            }
            else {
                m_Renderers = null;
            }
        }

        protected override Bounds GetMeshBounds() {
            if (m_Renderers.Length == 0)
                return new Bounds(Vector3.zero, Vector3.zero);

            Bounds compoundBounds = GetBoundsFor(m_Renderers[0]);
            for (int i = 1; i < m_Renderers.Length; ++i) {
                compoundBounds.Encapsulate(GetBoundsFor(m_Renderers[i]));
            }
            return compoundBounds;
        }

        private Bounds GetBoundsFor(Renderer ren) {
            if (ren is SkinnedMeshRenderer) {
                return ((SkinnedMeshRenderer)ren).localBounds;
            }
            return ren.bounds;
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

        protected override void SetSize(Vector3 inSize)
        {
            var meshSize = m_CachedMeshBounds.size;
            m_ModelTransform.localScale = new UnityEngine.Vector3(
                meshSize.x == 0 ? 1 : inSize.x/meshSize.x,
                meshSize.y == 0 ? 1 : inSize.y/meshSize.y,
                meshSize.z == 0 ? 1 : inSize.z/meshSize.z
            );

            // Inverse scale any holders on joints that may exist
            foreach(GameObject holderObject in FindAllInHierarchy(m_ModelTransform, "holder")){
                Transform holder = holderObject.transform;
                holder.localScale = new UnityEngine.Vector3(
                meshSize.x == 0 ? 1 : 1f / (inSize.x/meshSize.x),
                meshSize.y == 0 ? 1 : 1f / (inSize.y/meshSize.y),
                meshSize.z == 0 ? 1 : 1f / (inSize.z/meshSize.z)
                );
            }
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

        private List<GameObject> FindAllInHierarchy(Transform inTransform, string inName) {
            
            List<GameObject> objectsInHierarchy = new List<GameObject>();
            foreach(Transform child in inTransform.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.name == inName) {
                    objectsInHierarchy.Add(child.gameObject);
                }
            }
            return objectsInHierarchy;
        }
    }
}