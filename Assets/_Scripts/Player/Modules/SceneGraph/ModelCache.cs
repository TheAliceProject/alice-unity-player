using UnityEngine;
using System.Collections.Generic;
using Alice.Tweedle.File;

namespace Alice.Player.Unity {
    public sealed class ModelCache : ResourceCache<ModelSpec> {

        private Transform m_Root;

        public ModelCache(Transform inRoot) : base() {
            m_Root = inRoot;
            if (m_Root) {
                m_Root.gameObject.SetActive(false);
            }
        }

        public bool Add(string inIdentifier, GameObject inModel, Bounds inBounds, List<JointBounds> jointBounds) {
            var success = base.Add(inIdentifier, new ModelSpec(inModel, inBounds, jointBounds));
            NormalizeWeightsInModel(inModel);
            if (success && m_Root) {
                inModel.transform.SetParent(m_Root, false);
            }
            return success;
        }

        private void NormalizeWeightsInMesh(Mesh mesh)
        {
            var boneWeights = mesh.GetAllBoneWeights();
            var bonesPerVertex = mesh.GetBonesPerVertex();
            var boneWeightIndex = 0;
            
            for (var vertIndex = 0; vertIndex < mesh.vertexCount; vertIndex++)
            {
                var vertStartingBoneWeightIndex = boneWeightIndex;
                var numberOfBonesForThisVertex = bonesPerVertex[vertIndex];
                var totalWeight = 0f;

                if (numberOfBonesForThisVertex == 1)
                {
                    // No reason to normalize if there is only one bone
                    BoneWeight1 boneWeight = boneWeights[boneWeightIndex];
                    boneWeight.weight = 1.0f;
                    boneWeightIndex++;
                    continue;
                }

                for (var i = 0; i < numberOfBonesForThisVertex; i++) {
                    BoneWeight1 boneWeight = boneWeights[boneWeightIndex];
                    totalWeight += boneWeight.weight;
                    boneWeightIndex++;
                }

                // Backtrack to apply normalization
                boneWeightIndex = vertStartingBoneWeightIndex;

                for (var i = 0; i < numberOfBonesForThisVertex; i++) {
                    BoneWeight1 boneWeight = boneWeights[boneWeightIndex];
                    boneWeight.weight /= totalWeight;
                    boneWeightIndex++;
                }
            }
        }

        private void NormalizeWeightsInModel(GameObject model)
        {
            foreach(var rend in model.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                NormalizeWeightsInMesh(rend.sharedMesh);
            }
        }

    }
}