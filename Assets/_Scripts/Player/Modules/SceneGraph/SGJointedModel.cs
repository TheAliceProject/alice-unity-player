using UnityEngine;
using Alice.Tweedle;
using System.Collections.Generic;
using System;
using System.Linq;
using Alice.Player.Modules;

namespace Alice.Player.Unity {
    public sealed class SGJointedModel : SGModel {
        private struct RendererDetails
        {
            internal readonly BaseMaterial[] Materials;
            internal MaterialPropertyBlock[] Blocks;
            internal readonly Mesh bakedMesh;
            
            public RendererDetails(BaseMaterial[] material, MaterialPropertyBlock[] block, Mesh mesh) {
                Materials = material;
                Blocks = block;
                bakedMesh = mesh;
            }
        }
        private string m_ResourceId;
        private ModelSpec m_ModelSpec;
        private Renderer[] m_Renderers;
        private readonly Dictionary<Renderer, RendererDetails> m_Details = new Dictionary<Renderer, RendererDetails>();
        private readonly List<Transform> m_VehicledList = new List<Transform>();
        private SGJoint m_LeftEye;
        private SGJoint m_RightEye;
        private Mesh m_FaceMesh;

        private const float
        MinEyeU = -0.033f,
        MaxEyeU = .04f,
        MaxEyeV = .04f,
        MinEyeV = -.04f,
        URange = MaxEyeU - MinEyeU,
        VRange = MaxEyeV - MinEyeV;
        private const float MAX_EYE_LEFT_RIGHT = 1.0f;
        private const float MIN_EYE_LEFT_RIGHT = -1.0f;
        private const float MAX_EYE_UP_DOWN = .4f;
        private const float MIN_EYE_UP_DOWN = -0.4f;
        private const string leftEyeID = "LEFT_EYE";
        private const string rightEyeID = "RIGHT_EYE";

        struct UVData
        {
            public int idx;
            public float uVal;
            public float vVal;

            public UVData(float u, float v) {
                this.idx = -1;
                this.uVal = u;
                this.vVal = v;
            }
        }

        private List<UVData> leftEyeUVData;
        private List<UVData> rightEyeUVData;

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
                    BaseMaterial[] baseMaterials = new BaseMaterial[rend.sharedMaterials.Length];
                    MaterialPropertyBlock[] propertyBlocks = new MaterialPropertyBlock[rend.sharedMaterials.Length];

                    int i = 0;
                    foreach (var material in rend.sharedMaterials) { 
                        var mainTexture = material.mainTexture;

                        MaterialPropertyBlock propBlock = null;
                        GetPropertyBlock(rend, ref propBlock, i, false);
                        
	                    var baseMaterial = BaseMaterial.Opaque;
	                    if (mainTexture != null) {
	                        propBlock.SetTexture(MAIN_TEXTURE_SHADER_NAME, mainTexture);
	                        if (mainTexture is Texture2D texture && HasTranslucence(texture)) {
	                            baseMaterial = BaseMaterial.Transparent;
	                        }
	                    } else {
	                        baseMaterial = BaseMaterial.Glass;
	                    }

                        propertyBlocks[i] = propBlock;
                        baseMaterials[i] = baseMaterial;

                        i++;
                    }

                    Mesh bakedMesh = null;
                    if (rend is SkinnedMeshRenderer skinnedRenderer) {
                        // make sure the skinned mesh renderers local bounds get updated
                        skinnedRenderer.updateWhenOffscreen = true;

                        bakedMesh = new Mesh();
                        skinnedRenderer.BakeMesh(bakedMesh);
                    }

                    m_Details[rend] = new RendererDetails(baseMaterials, propertyBlocks, bakedMesh);
                    ApplyCurrentPaintAndOpacity(rend, ref propertyBlocks, baseMaterials);
                }
                CacheMeshBounds();
            }
            ResetColliderState();
        }

        private static bool HasTranslucence(Texture2D texture) {
            const double epsilon = 0.01;
            const double fractionOfAbsolutes = 0.84;

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
                ApplyPaint(rend, ref details.Blocks, details.Materials);
            }
        }

        protected override void OnOpacityChanged() {
            foreach (var rend in m_Renderers) {
                var details = m_Details[rend];
                ApplyOpacity(rend, ref details.Blocks, details.Materials);
            }
        }

        // This method should only be called on Sims models
        // It identifies the eye joint to update the uv mapping of the eyes when that eye joint moves.
        public void WatchEye(SGJoint eyeJoint) {
            if (leftEyeUVData == null || rightEyeUVData == null) {
                InitEyesUVData();
            }

            switch (eyeJoint.gameObject.name) {
                case leftEyeID:
                    m_LeftEye = eyeJoint;
                    break;
                case rightEyeID:
                    m_RightEye = eyeJoint;
                    break;
            }
        }

        public void JointChanged(SGJoint sgJoint) {
            if (sgJoint == m_LeftEye) {
                UpdateCoordinates(sgJoint, leftEyeUVData);
            }   
            if (sgJoint == m_RightEye) {
                UpdateCoordinates(sgJoint, rightEyeUVData);
            }   
        }

        private void UpdateCoordinates(SGJoint eyeJoint, List<UVData> uvData) {
            if (m_FaceMesh == null) {
                Debug.LogError("WatchEye: No face mesh found.");
                return;
            }
            var eyeAngles = eyeJoint.gameObject.transform.localEulerAngles;
            var curEyeLeftRight = RestrictAndConvertAngle(eyeAngles.y);
            var curEyeUpDown = RestrictAndConvertAngle(eyeAngles.x);
            
            var uOffset = (GetPercentBetween(curEyeLeftRight, MIN_EYE_LEFT_RIGHT, MAX_EYE_LEFT_RIGHT) - .5f) * URange;
            var vOffset = (GetPercentBetween(curEyeUpDown, MIN_EYE_UP_DOWN, MAX_EYE_UP_DOWN) - .5f) * VRange;

            var uvs = m_FaceMesh.uv;
            foreach (var uvDatum in uvData) {
                Vector2 curUV = uvs[uvDatum.idx];
                curUV.x = uvDatum.uVal + vOffset;
                curUV.y = uvDatum.vVal + uOffset;
                uvs[uvDatum.idx] = curUV;
            }
            m_FaceMesh.uv = uvs;
        }

        private static float RestrictAndConvertAngle(float degrees) {
            if (degrees > 180) {
                degrees -= 360;
            }
            degrees = Mathf.Clamp(degrees, -90, 90);
            return degrees * Mathf.Deg2Rad * -1;
        }

        float GetPercentBetween(float val, float min, float max) {
            if(val < min) {
                return 0;
            }
            if(val > max) {
                return 1;
            }
            return (val - min) / (max - min);
        }

        private void InitEyesUVData() {
            // UV Data for left eye
            leftEyeUVData = new List<UVData> {
                new UVData(0.033740f, 0.910210f),
                new UVData(0.059278f, 0.894269f),
                new UVData(0.038978f, 0.954912f),
                new UVData(0.059942f, 0.790487f),
                new UVData(0.081780f, 0.816221f),
                new UVData(0.059278f, 0.856990f),
                new UVData(0.081502f, 0.918293f),
                new UVData(0.091173f, 0.876598f),
                new UVData(0.035157f, 0.837395f)
            };

            // UV Data for right eye
            rightEyeUVData = new List<UVData> {
                new UVData(0.059278f, 0.856462f),
                new UVData(0.091993f, 0.874778f),
                new UVData(0.059278f, 0.895116f),
                new UVData(0.082230f, 0.935699f),
                new UVData(0.081954f, 0.834003f),
                new UVData(0.033548f, 0.841771f),
                new UVData(0.038860f, 0.796880f),
                new UVData(0.059558f, 0.960270f),
                new UVData(0.034869f, 0.913973f)
            };

            foreach (var r in m_Renderers) {
                Mesh mesh = r.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                Vector2[] uvs = mesh.uv;
                if (HasAllVertices(uvs, leftEyeUVData) && HasAllVertices(uvs, rightEyeUVData)) {
                    m_FaceMesh = mesh;
                    return;
                }
            }
        }

        private static bool HasAllVertices(Vector2[] uvs, List<UVData> eyeUvs) {
            for (var i = 0; i < eyeUvs.Count; i++) {
                var eyeUv = eyeUvs[i];
                for (var t = 0; t < uvs.Length; ++t) {
                    if (!Mathf.Approximately(uvs[t].x, eyeUv.uVal) ||
                        !Mathf.Approximately(uvs[t].y, eyeUv.vVal)) continue;
                    eyeUv.idx = t;
                    eyeUvs[i] = eyeUv;
                    break;
                }
                if (eyeUv.idx < 0) {
                    return false;
                }
            }

            return true;
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
            if (meshCollider == null) return;

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
            // Do not try to create colliders for things vehicled to this model
            if (!m_Renderers.Contains(skinnedRenderer)) return null;

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