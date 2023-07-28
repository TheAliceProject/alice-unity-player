
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class SGVRDevice : SGTransformableEntity {

        public override void CleanUp() {
        }

        public virtual void SetCamera(SGCamera cam) {
            vehicle = cam;
        }

        public override VantagePoint GetLocalTransformation() {
            return GetLocalTransformationUpTo(vehicle);
        }
    }
}