namespace Alice.Player.Unity {
    public sealed class SGVRHand : SGVRDevice {
        public override void SetCamera(SGCamera cam){
            base.SetCamera(cam);
            transform.SetParent(cam.GetHandFor(gameObject.name));
            if (cam.IsVRLoaded()) {
                transform.localPosition = UnityEngine.Vector3.zero;
            }
            else {
                transform.localPosition = new UnityEngine.Vector3(0, 1.014f, 0);
            }
        }
    }
}