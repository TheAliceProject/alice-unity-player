using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGJoint : SGTransformableEntity {
        private SGJointedModel m_parentModel;
        // To project from Sims joint space into Alice
        public VantagePoint modifier = VantagePoint.IDENTITY;
        // To project from Alice joint space into Sims
        public VantagePoint modifierInverse = VantagePoint.IDENTITY;

        public override void CleanUp(){}

        public void SetParentJointedModel(SGJointedModel parent){
            m_parentModel = parent;
        }

        public SGJointedModel GetParentJointedModel(){
            return m_parentModel;
        }

        protected override void StoreAliceTransformation(VantagePoint vp) {
            base.StoreAliceTransformation(vp.multiply(modifierInverse));
        }

        public override VantagePoint GetLocalTransformation() {
            return VantagePoint.FromUnity(cachedTransform.localPosition, cachedTransform.localRotation, modifier);
        }

        public override VantagePoint GetAbsoluteTransformation() {
            return VantagePoint.FromUnity(cachedTransform.position, cachedTransform.rotation, modifier);
        }

        public void SetReorientation(Orientation orientation) {
            modifier = new VantagePoint(orientation);
            modifierInverse = modifier.inverse();
        }
    }
}