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

        protected override void OnTransformationPropertyChanged(TValue inValue) {
            VantagePoint vp = inValue.RawObject<VantagePoint>();
            // convert to left-handedness

            // If the SGJointedModel has been scaled, inverse the scale of position to match
            UnityEngine.Vector3 scale = m_parentModel.transform.GetChild(0).localScale;
            cachedTransform.localPosition = new UnityEngine.Vector3(vp.UnityPosition().x / scale.x,
                                                                    vp.UnityPosition().y / scale.y,
                                                                    vp.UnityPosition().z / scale.z);
            cachedTransform.localRotation = vp.UnityRotation();
        }
    }
}