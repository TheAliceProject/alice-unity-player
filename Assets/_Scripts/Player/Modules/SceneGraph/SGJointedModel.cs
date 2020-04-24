using UnityEngine;
using Alice.Tweedle;
using System.Collections.Generic;
using System;
using Alice.Player.Modules;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {
        private string m_ResourceId;
        private ModelSpec m_ModelSpec;
        private Renderer[] m_Renderers;
        private MaterialPropertyBlock[] m_PropertyBlocks;
        private Dictionary<Renderer, Mesh> bakedMeshes = new Dictionary<Renderer, Mesh>();
        public List<Transform> m_vehicledList = new List<Transform>();

        public void SetResource(string inIdentifier) {
            if (m_ResourceId == inIdentifier) {
                return;
            }

            m_ResourceId = inIdentifier;
            var oldTransform = m_ModelTransform;

            m_ModelSpec = SceneGraph.Current.ModelCache.Get(inIdentifier);
            
            if (m_ModelSpec is null) {
                m_Renderers = null;
            } else {
                var model = Instantiate(m_ModelSpec.Model, cachedTransform, false);
                m_ModelTransform = model.transform;
                m_ModelTransform.localRotation = Quaternion.identity;
                m_ModelTransform.localPosition = Vector3.zero;
                
                if (oldTransform) {
                    CopySkeleton(oldTransform, m_ModelTransform);
                    Destroy(oldTransform.gameObject);
                }

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

                        bakedMeshes[skinnedRenderer] = new Mesh();
                        skinnedRenderer.BakeMesh(bakedMeshes[skinnedRenderer]);
                    }
                    ApplyCurrentPaintAndOpacity(m_Renderers[i], ref m_PropertyBlocks[i]);
                }
                CacheMeshBounds();
            }

            ResetColliderState();
        }

        private void CopySkeleton(Transform oldTransform, Transform newTransform) {
            m_ModelTransform.localScale = oldTransform.localScale;
            CopyJoints(FindInHierarchy(oldTransform, "ROOT"),
                FindInHierarchy(newTransform, "ROOT"));
        }
        

        private void CopyJoints(Transform oldTransform, Transform newTransform) {
            newTransform.localPosition = oldTransform.localPosition;
            newTransform.localRotation = oldTransform.localRotation;
            newTransform.localScale = oldTransform.localScale;
            foreach (Transform oldChild in oldTransform) {
                foreach (Transform newChild in newTransform) {
                    if (oldChild.gameObject.name == newChild.gameObject.name) {
                        CopyJoints(oldChild, newChild);
                    }
                }
            }
        }

        public void AddToVehicleList(Transform t)
        {
            m_vehicledList.Add(t);
        }

        protected override Bounds GetMeshBounds() {
            return m_ModelSpec.InitialBounds;
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
            if (bone == null) return null;
            var joint = SGEntity.Create<SGJoint>(inOwner, bone.gameObject);
            joint.SetParentJointedModel(this);
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

        protected override void CreateEntityCollider()
        {
            var root = FindInHierarchy(m_ModelTransform, "ROOT");
            if (root != null && CreateJointColliders(root)) {
                return;
            }

            // If no joint colliders were created, create colliders from meshes
            var skinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers != null)
            {
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    CreateMeshEntityCollider(skinnedMeshRenderer);
                }
            }
            base.CreateEntityCollider();
        }

        private bool CreateJointColliders(Transform inTransform) {
            var createdAny = false;
            if (m_ModelSpec.BoundsForJoint(inTransform.name, out var jointSize)) {
                if (inTransform.gameObject.GetComponent<Rigidbody>() == null) {
                    // Rigid body is required for collision detection between meshes
                    var rigidBody = inTransform.gameObject.AddComponent<Rigidbody>();
                    rigidBody.isKinematic = true;
                }
                var jointCollider = inTransform.gameObject.AddComponent<BoxCollider>();
                jointCollider.center = jointSize.center;
                jointCollider.size = jointSize.size;
                jointCollider.isTrigger = true;
                var broadcaster = inTransform.gameObject.AddComponent<CollisionBroadcaster>();
                broadcaster.SetColliderTarget(this);
                createdAny = true;
            }
            foreach (Transform child in inTransform) {
                if (CreateJointColliders(child)) {
                    createdAny = true;
                }
            }
            return createdAny;
        }

        private void CreateMeshEntityCollider(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            if (skinnedMeshRenderer.gameObject.GetComponent<Rigidbody>() != null) return;
            
            var meshCollider = CreateMeshCollider(skinnedMeshRenderer);
            // Rigid body is required for collision detection between meshes
            var rigidBody = skinnedMeshRenderer.gameObject.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
            skinnedMeshRenderer.gameObject.AddComponent<CollisionBroadcaster>();
        }

        protected override void CreateMouseCollider()
        {
            var skinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers != null)
            {
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    CreateMeshCollider(skinnedMeshRenderer);
                }
            }
            base.CreateMouseCollider();
        }

        private MeshCollider CreateMeshCollider(SkinnedMeshRenderer skinnedRenderer)
        {
            var meshCollider = skinnedRenderer.gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = bakedMeshes[skinnedRenderer];
            return meshCollider;
        }

        private void CollectInHierarchy(Transform inTransform, string start, List<string> matches) {
            foreach (Transform child in inTransform) {
                if (child.gameObject.name.StartsWith(start, StringComparison.Ordinal)) {
                    matches.Add(child.gameObject.name);
                }
                CollectInHierarchy(child, start, matches);
            }
        }

        private Transform FindInHierarchy(Transform inTransform, string inName) {
            foreach (Transform child in inTransform) {
                if (child.gameObject.name == inName) {
                    return child;
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