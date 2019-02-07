using UnityEngine;
namespace Alice.Player.Unity {
    public sealed class ModelCache : ResourceCache<GameObject> {

        private Transform m_Root;

        public ModelCache(Transform inRoot) : base() {
            m_Root = inRoot;
            if (m_Root) {
                m_Root.gameObject.SetActive(false);
            }
        }

        public override bool Add(string inIdentifier, GameObject inModel) {
            bool success = base.Add(inIdentifier, inModel);
            NormalizeWeightsInModel(inModel);
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