using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGJoint : SGTransformableEntity {
        private SGJointedModel m_parentModel;

        public override void CleanUp(){}

        public void SetParentJointedModel(SGJointedModel parent){
            m_parentModel = parent;
        }

        public SGJointedModel GetParentJointedModel(){
            return m_parentModel;
        }
    }
}