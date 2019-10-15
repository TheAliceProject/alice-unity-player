using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using BeauRoutine;
using System;

namespace Alice.Player.Unity {
    public sealed class SGCamera : SGTransformableEntity {
        public Camera Camera {get; private set;}
        public VRRig m_rig { get; private set; }

        protected override void Awake() {
            base.Awake();
            Camera = Camera.main;
            Camera.transform.SetParent(cachedTransform, false);
            Camera.transform.localPosition = UnityEngine.Vector3.zero;
            Camera.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
            Debug.Log("Found device? " + XRDevice.isPresent);
            if(VRControl.I.LoadWorldInVR)
            {
                Debug.Log("Found model: " + XRDevice.model);

                if (VRControl.I.LoadWorldInVR && VRControl.I.VRDeviceActive)
                {
                    Debug.Log("Loading VR Rig");
                    if (Camera.main != null)
                        Destroy(Camera.main.gameObject);
                    m_rig = Instantiate(SceneGraph.Current.InternalResources.VRRig, cachedTransform, false);
                    Camera = m_rig.headCamera;
                    Camera.tag = "MainCamera";
                    m_rig.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
                }
                else
                {
                    Camera = Camera.main;
                    Camera.transform.SetParent(cachedTransform, false);
                    Camera.transform.localPosition = UnityEngine.Vector3.zero;
                    Camera.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
                }
            }
                
        }

        public override void CleanUp() {
            if (Camera != null)
            {
                Camera.transform.SetParent(null, true);
                Camera = null;
            }
        }
    }
}