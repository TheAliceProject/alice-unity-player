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
            bool vrLoaded = false;
            if(VRControl.I.LoadWorldInVR)
            {
                if (XRDevice.isPresent)
                {
                    if (Camera.main != null)
                        Destroy(Camera.main.gameObject);
                    m_rig = Instantiate(SceneGraph.Current.InternalResources.VRRig, cachedTransform, false);
                    Camera = m_rig.headCamera;
                    Camera.tag = "MainCamera";
                    m_rig.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
                    vrLoaded = true;
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

        public override void CleanUp() {
            if (Camera != null)
            {
                Camera.transform.SetParent(null, true);
                Camera = null;
            }
        }
    }
}