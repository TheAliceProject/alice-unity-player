using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using BeauRoutine;
using System;
using System.Collections.Generic;
using Alice.Player.Primitives;
using Alice.Tweedle;
using Vector3 = UnityEngine.Vector3;

namespace Alice.Player.Unity {
    public sealed class SGCamera : SGTransformableEntity {
        public Camera Camera {get; private set;}
        public VRRig m_rig { get; private set; }

        private Routine routine;

        protected override void Awake() {
            base.Awake();
            if(IsVRLoaded())
            {
                if (Camera.main != null)
                    Destroy(Camera.main.gameObject);
                m_rig = Instantiate(SceneGraph.Current.InternalResources.VRRig, cachedTransform, false);
                Camera = m_rig.headCamera;
                Camera.tag = "MainCamera";
                if(VRControl.LoadedVRDevice() != VRControl.VRDevice.Vive)
                    m_rig.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
                VRControl.SetRig(m_rig);
                SceneGraph.Current.CreateVRCanvas();
                routine.Replace(this, CancelOutDefaultVrRotation());
            } else {
                Camera = Camera.main;
                Camera.transform.SetParent(cachedTransform, false);
                Camera.transform.localPosition = UnityEngine.Vector3.zero;
                Camera.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
            }
            Camera.GetComponentInChildren<AudioListener>().enabled = true;
            RegisterPropertyDelegate(SGModel.SIZE_PROPERTY_NAME, OnSizePropertyChanged);
        }

        public bool IsVRLoaded() {
            return VRControl.IsLoadedInVR() && IsXRDevicePresent();
        }

        public Transform GetHeadset() {
            return m_rig == null ? cachedTransform : m_rig.head;
        }

        public Transform GetHandFor(string handName)
        {
            if (m_rig == null)
            {
                return cachedTransform;
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

        private void OnSizePropertyChanged(TValue inValue) {
            if (m_rig == null) return;
            Vector3 scale = inValue.RawObject<Size>();
            m_rig.transform.SetScale(scale);
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

        private bool IsXRDevicePresent()
        {
            bool isPresent = false;
            List<XRDisplaySubsystem> xrDisplaySubsystems = new List<XRDisplaySubsystem>();

            SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
            foreach (var xrDisplay in xrDisplaySubsystems)
            {
                if (xrDisplay.running)
                {
                    isPresent = true;
                }
            }
            return isPresent;
        }
    }
}