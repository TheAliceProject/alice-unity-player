using Alice.Tweedle;

namespace Alice.Player.Unity {
    public sealed class SGJoint : SGTransformableEntity {
        private SGJointedModel m_parentModel;

        public override void CleanUp(){}

        protected override void OnTransformationPropertyChanged(TValue inValue) {
            base.OnTransformationPropertyChanged(inValue);
            m_parentModel.JointChanged(this);
        }
        
        public void SetParentJointedModel(SGJointedModel parent){
            m_parentModel = parent;
        }

        public SGJointedModel GetParentJointedModel(){
            return m_parentModel;
        }
    }
}