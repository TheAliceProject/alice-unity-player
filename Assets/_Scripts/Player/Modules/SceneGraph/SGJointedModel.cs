using UnityEngine;
using Alice.Tweedle;
using System.Collections.Generic;
using System;
using Alice.Player.Modules;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {
        private struct RendererDetails
        {
            internal readonly BaseMaterial Material;
            internal MaterialPropertyBlock Block;
            internal readonly Mesh bakedMesh;
            
            public RendererDetails(BaseMaterial material, MaterialPropertyBlock block, Mesh mesh) {
                Material = material;
                Block = block;
                bakedMesh = mesh;
            }
        }
        private string m_ResourceId;
        private ModelSpec m_ModelSpec;
        private Renderer[] m_Renderers;
        private readonly Dictionary<Renderer, RendererDetails> m_Details = new Dictionary<Renderer, RendererDetails>();
        private readonly List<Transform> m_VehicledList = new List<Transform>();

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

                foreach (var rend in m_Renderers) {
                    MaterialPropertyBlock propBlock = null;
                    GetPropertyBlock(rend, ref propBlock);

                    var mainTexture = rend.sharedMaterial.mainTexture;
                    var baseMaterial = BaseMaterial.Opaque;
                    if (mainTexture != null) {
                        propBlock.SetTexture(MAIN_TEXTURE_SHADER_NAME, mainTexture);
                        if (mainTexture is Texture2D texture && HasTranslucence(texture)) {
                            baseMaterial = BaseMaterial.Transparent;
                        }
                    } else {
                        rend.sharedMaterial = SceneGraph.Current.InternalResources.GlassMaterial;
                        baseMaterial = BaseMaterial.Glass;
                    }

                    Mesh bakedMesh = null;
                    if (rend is SkinnedMeshRenderer skinnedRenderer) {
                        // make sure the skinned mesh renderers local bounds get updated
                        skinnedRenderer.updateWhenOffscreen = true;

                        bakedMesh = new Mesh();
                        skinnedRenderer.BakeMesh(bakedMesh);
                    }
                    m_Details[rend] = new RendererDetails(baseMaterial, propBlock, bakedMesh);
                    ApplyCurrentPaintAndOpacity(rend, ref propBlock, baseMaterial);
                }
                CacheMeshBounds();
            }
            ResetColliderState();
        }

        private static bool HasTranslucence(Texture2D texture) {
            const double epsilon = 0.01;
            const double fractionOfAbsolutes = 0.96;

            var colors = texture.GetPixels();
            var total = colors.Length;
            if (total == 0) {
                return true;
            }
            var absolutes = 0;
            for (var i = 0; i < total; i++) {
                if (Math.Abs(colors[i].a - 1.0) < epsilon || colors[i].a < epsilon) {
                    absolutes++;
                }
            }

            return (double) absolutes / total < fractionOfAbsolutes;
        }

        private void CopySkeleton(Transform oldTransform, Transform newTransform) {
            m_ModelTransform.localScale = oldTransform.localScale;
            var currentRoot = FindInHierarchy(oldTransform, "ROOT");
            if (currentRoot == null) return;
            CopyJoints(currentRoot, FindInHierarchy(newTransform, "ROOT"));
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

        public void AddToVehicleList(Transform t) {
            m_VehicledList.Add(t);
        }

        protected override Bounds GetMeshBounds() {
            return m_ModelSpec.InitialBounds;
        }

        protected override void OnPaintChanged() {
            foreach (var rend in m_Renderers) {
                var details = m_Details[rend];
                ApplyPaint(rend, ref details.Block, details.Material);
            }
        }

        protected override void OnOpacityChanged() {
            foreach (var rend in m_Renderers) {
                var details = m_Details[rend];
                ApplyOpacity(rend, ref details.Block, details.Material);
            }
        }

        public SGJoint LinkJoint(TValue inOwner, string inName) {
            var bone = FindInHierarchy(m_ModelTransform, inName);
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
            var xScale = meshSize.x == 0 ? 1 : inSize.x/meshSize.x;
            var yScale = meshSize.y == 0 ? 1 : inSize.y/meshSize.y;
            var zScale = meshSize.z == 0 ? 1 : inSize.z/meshSize.z;
            m_ModelTransform.localScale = new Vector3(xScale, yScale, zScale);
            // Inverse scale any holders on joints that may exist
            foreach(var holder in m_VehicledList){
                holder.localScale = new Vector3(1/xScale, 1/yScale, 1/zScale);
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
            meshCollider.sharedMesh = m_Details[skinnedRenderer].bakedMesh;
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
                if (child.gameObject.name.Equals(inName, StringComparison.OrdinalIgnoreCase)) {
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