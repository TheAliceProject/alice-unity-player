using System.Collections.Generic;
using Alice.Tweedle.File;
using UnityEngine;

namespace Alice.Player.Unity
{
    public class ModelSpec : Object
    {
        public readonly GameObject Model;
        public Bounds InitialBounds;
        private readonly Dictionary<string, Bounds> m_InitialJointBounds = new Dictionary<string, Bounds>();

        public ModelSpec(GameObject model, Bounds initialBounds, IReadOnlyCollection<JointBounds> jointBounds)
        {
            Model = model;
            InitialBounds = initialBounds;
            if (jointBounds == null) return;
            foreach (var joint in jointBounds)
            {
                m_InitialJointBounds[joint.name] = joint.bounds.AsBounds();
            }
        }

        public bool BoundsForJoint(string joint, out Bounds bounds)
        {
            return m_InitialJointBounds.TryGetValue(joint, out bounds);
        }
    }
}