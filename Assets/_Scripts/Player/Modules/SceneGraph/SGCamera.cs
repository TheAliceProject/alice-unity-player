using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using BeauRoutine;
using System;

namespace Alice.Player.Unity {
    public sealed class SGCamera : SGTransformableEntity {
        public Camera Camera {get; private set;}
        public VRRig m_rig { get; private set; }

        private Routine routine;

        protected override void Awake() {
            base.Awake();
            bool vrLoaded = false;
            if(VRControl.IsLoadedInVR())
            {
                if (XRDevice.isPresent)
                {
                    if (Camera.main != null)
                        Destroy(Camera.main.gameObject);
                    m_rig = Instantiate(SceneGraph.Current.InternalResources.VRRig, cachedTransform, false);
                    Camera = m_rig.headCamera;
                    Camera.tag = "MainCamera";
                    if(VRControl.LoadedVRDevice() != VRControl.VRDevice.Vive)
                        m_rig.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
                    VRControl.SetRig(m_rig);
                    vrLoaded = true;
                    routine.Replace(this, CancelOutDefaultVrRotation());
                }
            }
            
            if(!vrLoaded)
            {
                Camera = Camera.main;
                Camera.transform.SetParent(cachedTransform, false);
                Camera.transform.localPosition = UnityEngine.Vector3.zero;
                Camera.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
            }
        }

        public Transform GetHandFor(string handName)
        {
            if (m_rig == null)
            {
                return Camera.transform;
            }
            if (handName.Contains("Left"))
            {
                return m_rig.leftController;
            }
            if (handName.Contains("Right"))
            {
                return m_rig.rightController;
            }
            throw new ArgumentException("No recognized hand in " + handName);
        }

        private IEnumerator CancelOutDefaultVrRotation()
        {
            // See comment in VRControl about this function
            float startTime = Time.time;
            while(Time.time - startTime < 1f)
            {
                float currAngle = normalizeAngle(cachedTransform.localRotation.eulerAngles.x);
                if (currAngle >= -VRControl.INITIAL_CAMERA_ANGLE_CUTOFF &&
                    currAngle < VRControl.INITIAL_CAMERA_ANGLE_CUTOFF){
                    cachedTransform.localRotation = UnityEngine.Quaternion.Euler(0f, cachedTransform.localEulerAngles.y, cachedTransform.localEulerAngles.z);
                    break;
                }
                yield return null;
            }
            yield return null;
        }

        public override void CleanUp() {
            if (Camera != null)
            {
                Camera.transform.SetParent(null, true);
                Camera = null;
            }
        }

        // This helper function is because sometimes in Unity, negative angles might look like -10, or 350
        float normalizeAngle(float angle)
        { 
            float newAngle = angle % 360f;
            if (newAngle > 180f)
                newAngle -= 360f;
            return newAngle;
        }
    }
}