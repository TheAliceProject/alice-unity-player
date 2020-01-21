using UnityEngine;
using Alice.Tweedle;
using System.Collections.Generic;
using System;
using Alice.Player.Modules;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {

        private const float RootCapsuleRadius = 0.6f;
        private const float CapsuleThinningFactor = 0.91f;

        private string m_ResourceId;
        private Bounds m_modelBounds;
        private Renderer[] m_Renderers;
        private MaterialPropertyBlock[] m_PropertyBlocks;
        public List<Transform> m_vehicledList = new List<Transform>();

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
                m_modelBounds = SceneGraph.Current.ModelCache.GetBoundingBoxFromModel(inIdentifier);
                m_ModelTransform = model.transform;
                m_ModelTransform.localRotation = UnityEngine.Quaternion.identity;
                m_ModelTransform.localPosition = UnityEngine.Vector3.zero;

                m_Renderers = model.GetComponentsInChildren<Renderer>();
                m_PropertyBlocks = new MaterialPropertyBlock[m_Renderers.Length];

                for (int i = 0; i < m_Renderers.Length; ++i) {
                    GetPropertyBlock(m_Renderers[i], ref m_PropertyBlocks[i]);
                    if (m_Renderers[i].sharedMaterial.mainTexture != null)
                        m_PropertyBlocks[i].SetTexture(MAIN_TEXTURE_SHADER_NAME, m_Renderers[i].sharedMaterial.mainTexture);
                    else
                        Debug.LogWarningFormat("SetTexture '{0}' is null for {1}", MAIN_TEXTURE_SHADER_NAME, model);

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

        public void AddToVehicleList(Transform t)
        {
            m_vehicledList.Add(t);
        }

        protected override Bounds GetMeshBounds() {
            return m_modelBounds;
        }

        protected override void OnPaintChanged() {
            for (int i = 0; i < m_Renderers?.Length; ++i) {
                ApplyPaint(m_Renderers[i], ref m_PropertyBlocks[i]);
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
                joint.SetParentJointedModel(this);
            }
            return joint;
        }

        internal string[] FindJointsBeginningWith(string start) {
            List<string> matches = new List<string>();
            CollectInHierarchy(m_ModelTransform, start, matches);
            return matches.ToArray();
        }

        protected override void SetSize(Vector3 inSize) {
            var meshSize = m_CachedMeshBounds.size;
            m_ModelTransform.localScale = new UnityEngine.Vector3(
                meshSize.x == 0 ? 1 : inSize.x/meshSize.x,
                meshSize.y == 0 ? 1 : inSize.y/meshSize.y,
                meshSize.z == 0 ? 1 : inSize.z/meshSize.z
            );

            // Inverse scale any holders on joints that may exist
            foreach(Transform holder in m_vehicledList){
                holder.localScale = new UnityEngine.Vector3(
                meshSize.x == 0 ? 1 : meshSize.x/inSize.x,
                meshSize.y == 0 ? 1 : meshSize.y/inSize.y,
                meshSize.z == 0 ? 1 : meshSize.z/inSize.z
                );
            }
        }
        public override void AddCollider() {
            var root = FindInHierarchy(m_ModelTransform, "ROOT");
            AddJointColliders(root.transform, RootCapsuleRadius);
        }

        private void AddJointColliders(Transform inTransform, float radius, Transform parent = null) {
            if (parent != null) {
                var jointLength = inTransform.localPosition.magnitude;
                if (jointLength > Single.Epsilon)
                {
                    CapsuleCollider jointCollider = parent.gameObject.AddComponent<CapsuleCollider>();
                    jointCollider.radius = jointLength * radius;
                    jointCollider.height = jointLength * (1f + 2f * radius);
                    jointCollider.direction = 2; // The Z-Axis, which aims at the next joint
                    jointCollider.isTrigger = true;
                    CollisionBroadcaster broadcaster = parent.gameObject.AddComponent<CollisionBroadcaster>();
                    broadcaster.SetColliderTarget(this);
                }
            }
            foreach (Transform child in inTransform) {
                AddJointColliders(child, radius * CapsuleThinningFactor, inTransform);
            }
        }
        

        private void CollectInHierarchy(Transform inTransform, string start, List<string> matches) {
            foreach (Transform child in inTransform) {
                if (child.gameObject.name.StartsWith(start, StringComparison.Ordinal)) {
                    matches.Add(child.gameObject.name);
                }
                CollectInHierarchy(child, start, matches);
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
    }
}