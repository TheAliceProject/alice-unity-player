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
            //Debug.Log(inModel.GetComponent<MeshRenderer>().materials[2].color.a);
            if (success && m_Root) {
                inModel.transform.SetParent(m_Root, false);
            }
            return success;
        }

        private void NormalizeWeightsInMesh(Mesh mesh)
        {
            BoneWeight[] newWeights = mesh.boneWeights;
            for (int i = 0; i < mesh.boneWeights.Length; i++)
            {
                BoneWeight weight = newWeights[i];
                double weightTotal = weight.weight0 + weight.weight1 + weight.weight2 + weight.weight3;
                weight.weight0 = (float)(weight.weight0 / weightTotal);
                weight.weight1 = (float)(weight.weight1 / weightTotal);
                weight.weight2 = (float)(weight.weight2 / weightTotal);
                weight.weight3 = (float)(weight.weight3 / weightTotal);
                newWeights[i] = weight;
            }
            mesh.boneWeights = newWeights;
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