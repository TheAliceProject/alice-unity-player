namespace Alice.Player.Unity {
    public class SGVRHeadset : SGVRDevice {
        private SGCamera m_camera;

        public SGCamera GetCamera() {
            return m_camera;
        }

        public override void SetCamera(SGCamera cam) {
            m_camera = cam;
            base.SetCamera(cam);
            transform.SetParent(cam.GetHeadset());
            if (!cam.IsVRLoaded()) {
                cam.Camera.transform.SetParent(transform, false);
                transform.localPosition = new UnityEngine.Vector3(0, 1.561f, 0);
                transform.localRotation = new UnityEngine.Quaternion(-0.098017f, 0, 0, 0.995185f);
            }
        }
    }
}