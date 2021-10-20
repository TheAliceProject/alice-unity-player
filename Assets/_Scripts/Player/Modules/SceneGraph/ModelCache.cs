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

            if (success && m_Root) {
                inModel.transform.SetParent(m_Root, false);
            }
            return success;
        }

    }
}