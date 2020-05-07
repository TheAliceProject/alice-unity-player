using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGVRHand : SGTransformableEntity {
        private SGCamera m_camera;

        public override void CleanUp(){}

        public void SetCamera(SGCamera camera){
            m_camera = camera;
            vehicle = camera;
            transform.SetParent(camera.GetHandFor(gameObject.name));
            transform.localPosition = UnityEngine.Vector3.zero;
        }

        public SGCamera GetCamera(){
            return m_camera;
        }

        public override VantagePoint GetLocalTransformation() {
            return GetLocalTransformationUpTo(m_camera);
        }
    }
}