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
        private readonly List<Transform> m_JointRiders = new List<Transform>();
        private SGJoint m_LeftEye;
        private SGJoint m_RightEye;
        private SGJoint m_LeftEyelid;
        private SGJoint m_RightEyelid;
        private SGJoint m_Mouth;
        private SkinnedMeshRenderer m_FaceRenderer;
        private SkinnedMeshRenderer m_LowerTeethRenderer;

        private const float
        MIN_EYE_U = -0.033f,
        MAX_EYE_U = .04f,
        MIN_EYE_V = -.04f,
        MAX_EYE_V = .04f,
        U_RANGE = MAX_EYE_U - MIN_EYE_U,
        V_RANGE = MAX_EYE_V - MIN_EYE_V;
        private const float MAX_EYE_LEFT_RIGHT = 1.0f;
        private const float MIN_EYE_LEFT_RIGHT = -1.0f;
        private const float MAX_EYE_UP_DOWN = .4f;
        private const float MIN_EYE_UP_DOWN = -0.4f;
        private const float MAX_EYELID_UP_DOWN = 0.0f;
        private const float MIN_EYELID_UP_DOWN = -1.2f;
        private const float MAX_MOUTH_UP_DOWN = 0.0f;
        private const float MIN_MOUTH_UP_DOWN = -1.0f;
        private const string leftEyeID = "LEFT_EYE";
        private const string rightEyeID = "RIGHT_EYE";
        private const string leftEyelidID = "LEFT_EYELID";
        private const string rightEyelidID = "RIGHT_EYELID";
        private const string mouthID = "MOUTH";

        private readonly List<Vector2> _leftEyeUVData = new List<Vector2> {
            new Vector2(0.033740f, 0.08978999f),
            new Vector2(0.059278f, 0.105731f),
            new Vector2(0.038978f, 0.04508799f),
            new Vector2(0.059942f, 0.209513f),
            new Vector2(0.081780f, 0.183779f),
            new Vector2(0.059278f, 0.14301f),
            new Vector2(0.081502f, 0.081707f),
            new Vector2(0.091173f, 0.123402f),
            new Vector2(0.035157f, 0.162605f)
        };
        private readonly List<Vector2> _rightEyeUVData = new List<Vector2> {
            new Vector2(0.059278f, 0.143538f),
            new Vector2(0.091993f, 0.125222f),
            new Vector2(0.059278f, 0.104884f),
            new Vector2(0.082230f, 0.06430101f),
            new Vector2(0.081954f, 0.165997f),
            new Vector2(0.033548f, 0.158229f),
            new Vector2(0.038860f, 0.20312f),
            new Vector2(0.059558f, 0.03973001f),
            new Vector2(0.034869f, 0.08602703f)
        };

        private readonly Dictionary<int,Vector2> _leftIndices = new Dictionary<int, Vector2>();
        private readonly Dictionary<int,Vector2> _rightIndices = new Dictionary<int, Vector2>();

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

        public void AddJointRider(Transform t) {
            m_JointRiders.Add(t);
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

        // This method is invoked by interop method trackFacialJoint and should only be called on Sims models
        // It identifies a joint that is not simply weighted to, and instead affects either:
        //  - uv mapping, in the case of eye joints
        //  - blendshapes, for mouth and eyelid joints
        public void StartTrackingJoint(SGJoint inputJoint) {
            if (m_FaceRenderer == null) {
                IdentifyFaceMeshes();
            }

            switch (inputJoint.gameObject.name)
            {
                case leftEyeID:
                    m_LeftEye = inputJoint;
                    break;
                case rightEyeID:
                    m_RightEye = inputJoint;
                    break;
                case leftEyelidID:
                    m_LeftEyelid = inputJoint;
                    break;
                case rightEyelidID:
                    m_RightEyelid = inputJoint;
                    break;
                case mouthID:
                    m_Mouth = inputJoint;
                    break;
            }
        }

        public void JointChanged(SGJoint sgJoint) {
            if (m_FaceRenderer == null) {
                return;
            }
            if (sgJoint == m_LeftEye) {
                UpdateEyeTexture(sgJoint, _leftIndices);
            }   
            if (sgJoint == m_RightEye) {
                UpdateEyeTexture(sgJoint, _rightIndices);
            }
            if (sgJoint == m_LeftEyelid) {
                UpdateEyelidBlendShape(sgJoint, 2);
            }
            if (sgJoint == m_RightEyelid) {
                UpdateEyelidBlendShape(sgJoint, 1);
            }
            if (sgJoint == m_Mouth) {
                UpdateMouthBlendShapes(sgJoint);
            }
        }

        private void UpdateEyeTexture(SGJoint eyeJoint, Dictionary<int, Vector2> uvData) {
            var eyeAngles = eyeJoint.gameObject.transform.localEulerAngles;
            var curEyeLeftRight = RestrictAndConvertAngle(eyeAngles.y);
            var curEyeUpDown = 0 - RestrictAndConvertAngle(eyeAngles.x);
            var uOffset = (GetFractionBetween(curEyeLeftRight, MIN_EYE_LEFT_RIGHT, MAX_EYE_LEFT_RIGHT) - .5f) * U_RANGE;
            var vOffset = (GetFractionBetween(curEyeUpDown, MIN_EYE_UP_DOWN, MAX_EYE_UP_DOWN) - .5f) * V_RANGE;

            var faceMesh = m_FaceRenderer.sharedMesh;
            var uvs = faceMesh.uv;
            foreach (var uvDatum in uvData) {
                var curUV = uvs[uvDatum.Key];
                curUV.x = uvDatum.Value.x + vOffset;
                curUV.y = uvDatum.Value.y + uOffset;
                uvs[uvDatum.Key] = curUV;
            }
            faceMesh.uv = uvs;
        }

        private void UpdateEyelidBlendShape(SGJoint eyelidJoint, int meshId) {
            var eyelidAngles = eyelidJoint.gameObject.transform.localEulerAngles;
            var curEyeLidUpDown = RestrictAndConvertAngle(eyelidAngles.x);
            var blendWeight = 1f - GetFractionBetween(curEyeLidUpDown, MIN_EYELID_UP_DOWN, MAX_EYELID_UP_DOWN);
            m_FaceRenderer.SetBlendShapeWeight(meshId, blendWeight);
        }

        private void UpdateMouthBlendShapes(SGJoint mouthJoint) {
            var mouthAngles = mouthJoint.gameObject.transform.localEulerAngles;
            var curMouthUpDown = RestrictAndConvertAngle(mouthAngles.x);
            var blendWeight = 1f - GetFractionBetween(curMouthUpDown, MIN_MOUTH_UP_DOWN, MAX_MOUTH_UP_DOWN);
            m_FaceRenderer.SetBlendShapeWeight(0, blendWeight);
            m_LowerTeethRenderer.SetBlendShapeWeight(0, blendWeight / 2);
        }

        private static float RestrictAndConvertAngle(float degrees) {
            if (degrees > 180) {
                degrees -= 360;
            }
            degrees = Mathf.Clamp(degrees, -90, 90);
            return degrees * Mathf.Deg2Rad;
        }

        private static float GetFractionBetween(float val, float min, float max) {
            if(val < min) {
                return 0;
            }
            if(val > max) {
                return 1;
            }
            return (val - min) / (max - min);
        }

        private void IdentifyFaceMeshes() {
            foreach (var r in m_Renderers) {
                var meshRenderer = r.GetComponent<SkinnedMeshRenderer>();
                var mesh = meshRenderer.sharedMesh;
                if (mesh.blendShapeCount == 2) {
                    if (m_LowerTeethRenderer != null) {
                        Debug.LogError("Two teeth meshes");
                    }
                    m_LowerTeethRenderer = meshRenderer;
                }
                if (mesh.blendShapeCount == 3) {
                    if (m_FaceRenderer != null) {
                        Debug.LogError("Two face meshes");
                    }
                    m_FaceRenderer = meshRenderer;
                    IdentifyEyeUvs(mesh.uv);
                }
            }
        }

        private void IdentifyEyeUvs(Vector2[] uvs) {
            if (uvs == null) {
                Debug.LogError("No UV values found.");
                return;
            }
            _leftIndices.Clear();
            _rightIndices.Clear();
            for (var index = 0; index < uvs.Length; index++) {
                var uv = uvs[index];
                if (IsInVertices(uv, _leftEyeUVData)) {
                    _leftIndices.Add(index, uv);
                } else if (IsInVertices(uv, _rightEyeUVData)) {
                    _rightIndices.Add(index, uv);
                }
            }
        }

        private static bool IsInVertices(Vector2 uv, List<Vector2> uvData) {
            return uvData.Any(eyeUv => Mathf.Approximately(uv.x, eyeUv.x) && Mathf.Approximately(uv.y, eyeUv.y));
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
            foreach(var rider in m_JointRiders){
                var oldScale = rider.localScale;
                var pos= rider.localPosition;
                // Replace previous scale with new scale
                rider.localPosition = new Vector3(pos.x/(xScale*oldScale.x),
                                                  pos.y/(yScale*oldScale.y),
                                                  pos.z/(zScale*oldScale.z));
                // Invert joint scale on riders
                rider.localScale = new Vector3(1/xScale, 1/yScale, 1/zScale);
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