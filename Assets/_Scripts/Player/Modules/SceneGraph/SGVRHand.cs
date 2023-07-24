namespace Alice.Player.Unity {
    public sealed class SGVRHand : SGVRDevice {
        public override void SetCamera(SGCamera cam){
            base.SetCamera(cam);
            transform.SetParent(cam.GetHandFor(gameObject.name));
            transform.localPosition = UnityEngine.Vector3.zero;
        }
    }
}