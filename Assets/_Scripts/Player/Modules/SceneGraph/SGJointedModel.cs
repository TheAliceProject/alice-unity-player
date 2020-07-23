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

        // For Sims models, there will be several meshes and the 4th one is the face mesh.
        private int faceMeshIdx = -1;
        private const float
        sMinEyeU = -0.033f,
        sMaxEyeU = .04f,
        sMaxEyeV = .04f,
        sMinEyeV = -.04f;
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

            public UVData(int i, float u, float v) {
                this.idx = i;
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
        // And when being called, this method updates the uv mapping of the eyes based on the eye joints rotation.
        public void OnEyesChanged(SGJoint eyeJoint) {
            if(leftEyeUVData == null || rightEyeUVData == null) {
                InitEyesUVData();
            }
            if(faceMeshIdx < 0) {
                Debug.LogError("OnEyesChanged: No proper mesh found.");
                return;
            }
            List<UVData> curEyeUVData;
            if(eyeJoint.gameObject.name == leftEyeID) {
                curEyeUVData = leftEyeUVData;
            }
            else if(eyeJoint.gameObject.name == rightEyeID) {
                curEyeUVData = rightEyeUVData;
            }
            else {
                return;
            }

            float curEyeLeftRight = eyeJoint.gameObject.transform.localEulerAngles.y;
            if(curEyeLeftRight > 180) {
                curEyeLeftRight -= 360;
            }
            curEyeLeftRight = Mathf.Clamp(curEyeLeftRight, -90, 90);
            curEyeLeftRight *= Mathf.Deg2Rad * -1;

            float curEyeUpDown = eyeJoint.gameObject.transform.localEulerAngles.x;
            if(curEyeUpDown > 180) {
                curEyeUpDown -= 360;
            }
            curEyeUpDown = Mathf.Clamp(curEyeUpDown, -90, 90);
            curEyeUpDown *= Mathf.Deg2Rad * -1;

            float uRange = sMaxEyeU - sMinEyeU;
            float vRange = sMaxEyeV - sMinEyeV;

            float uOffset = (GetPercentBetween(curEyeLeftRight, MIN_EYE_LEFT_RIGHT, MAX_EYE_LEFT_RIGHT) - .5f) * uRange;
            float vOffset = (GetPercentBetween(curEyeUpDown, MIN_EYE_UP_DOWN, MAX_EYE_UP_DOWN) - .5f) * vRange;

            Mesh mesh = m_Renderers[faceMeshIdx].GetComponent<SkinnedMeshRenderer>().sharedMesh;
            Vector2[] uvs = mesh.uv;
            for(int i = 0; i < curEyeUVData.Count; ++i) {
                Vector2 curUV = uvs[curEyeUVData[i].idx];
                curUV.x = curEyeUVData[i].uVal + vOffset;
                curUV.y = curEyeUVData[i].vVal + uOffset;
                uvs[curEyeUVData[i].idx] = curUV;
            }
            mesh.uv = uvs;
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
            leftEyeUVData = new List<UVData>();
            leftEyeUVData.Add(new UVData(136, 0.033740f, 0.910210f));
            leftEyeUVData.Add(new UVData(137, 0.059278f, 0.894269f));
            leftEyeUVData.Add(new UVData(138, 0.038978f, 0.954912f));
            leftEyeUVData.Add(new UVData(175, 0.059942f, 0.790487f));
            leftEyeUVData.Add(new UVData(176, 0.081780f, 0.816221f));
            leftEyeUVData.Add(new UVData(177, 0.059278f, 0.856990f));
            leftEyeUVData.Add(new UVData(178, 0.081502f, 0.918293f));
            leftEyeUVData.Add(new UVData(179, 0.091173f, 0.876598f));
            leftEyeUVData.Add(new UVData(180, 0.035157f, 0.837395f));

            // UV Data for right eye
            rightEyeUVData = new List<UVData>();
            rightEyeUVData.Add(new UVData(181, 0.059278f, 0.856462f));
            rightEyeUVData.Add(new UVData(182, 0.091993f, 0.874778f));
            rightEyeUVData.Add(new UVData(183, 0.059278f, 0.895116f));
            rightEyeUVData.Add(new UVData(184, 0.082230f, 0.935699f));
            rightEyeUVData.Add(new UVData(185, 0.081954f, 0.834003f));
            rightEyeUVData.Add(new UVData(186, 0.033548f, 0.841771f));
            rightEyeUVData.Add(new UVData(187, 0.038860f, 0.796880f));
            rightEyeUVData.Add(new UVData(188, 0.059558f, 0.960270f));
            rightEyeUVData.Add(new UVData(189, 0.034869f, 0.913973f));

            bool eyeMeshFound = false;
            int curIdx = 0;

            while(!eyeMeshFound && curIdx < m_Renderers.Length) {
                Mesh mesh = m_Renderers[curIdx].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                Vector2[] uvs = mesh.uv;
                // Find a candidate mesh that has one pair match.
                for(int i = 0; i < uvs.Length; ++i) {
                    
                    if(Mathf.Approximately(uvs[i].x, leftEyeUVData[0].uVal) 
                        && Mathf.Approximately(uvs[i].y, leftEyeUVData[0].vVal)) {
                        break;
                    }
                }
                // Check the candidate for all the pairs.
                bool foundAllMatchForLeft = true;
                for(int j = 0; j < leftEyeUVData.Count; ++j) {
                    bool foundCurMatch = false;
                    for(int t = 0; t < uvs.Length; ++t) {
                        if(Mathf.Approximately(uvs[t].x, leftEyeUVData[j].uVal)
                        && Mathf.Approximately(uvs[t].y, leftEyeUVData[j].vVal)) {
                            foundCurMatch = true;
                            UVData tmp = leftEyeUVData[j];
                            tmp.idx = t;
                            leftEyeUVData[j] = tmp;
                            break;
                        }
                    }
                    if(!foundCurMatch) { // no long a candidate
                        foundAllMatchForLeft = false;
                        break;
                    }
                }
                if(!foundAllMatchForLeft) {
                    curIdx++;
                    continue;
                }
                bool foundAllMatchForRight = true;
                for(int k = 0; k < rightEyeUVData.Count; k++) {
                    bool foundCurMatch = false;
                    for(int t = 0; t < uvs.Length; ++t) {
                        if(Mathf.Approximately(uvs[t].x, rightEyeUVData[k].uVal)
                        && Mathf.Approximately(uvs[t].y, rightEyeUVData[k].vVal)) {
                            foundCurMatch = true;
                            UVData tmp = rightEyeUVData[k];
                            tmp.idx = t;
                            rightEyeUVData[k] = tmp;
                            break;
                        }
                    }
                    if(!foundCurMatch) {
                        foundAllMatchForRight = false;
                        break;
                    }
                }
                if(!foundAllMatchForRight) {
                    curIdx++;
                    continue;
                }
                faceMeshIdx = curIdx;
                return;
            }
        }


        // For test ONLY
        //private void Update() {
        //    if(Input.GetKeyDown(KeyCode.J)) {
        //        GameObject leftEye = GameObject.Find("LEFT_EYE");
        //        OnEyesChanged(leftEye.GetComponent<SGJoint>());
        //    }
        //    if(Input.GetKeyDown(KeyCode.K)) {
        //        GameObject rightEye = GameObject.Find("RIGHT_EYE");
        //        OnEyesChanged(rightEye.GetComponent<SGJoint>());
        //    }
        //}

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