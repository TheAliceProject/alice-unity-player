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
            Routine.Start(InstantiateCameraRoutine());
        }

        public override void CleanUp() {
            if (Camera != null)
            {
                Camera.transform.SetParent(null, true);
                Camera = null;
            }
        }

        IEnumerator InstantiateCameraRoutine()
        {
            for (int i = 0; i < XRSettings.supportedDevices.Length; i++)
            {
                yield return EnableVRRoutine(XRSettings.supportedDevices[i]);
                if(XRSettings.enabled)
                    break;
            }

            if(XRSettings.isDeviceActive && XRSettings.enabled){
                if(Camera.main != null)
                    Destroy(Camera.main.gameObject);
                m_rig = Instantiate(SceneGraph.Current.InternalResources.VRRig, cachedTransform, false);
                Camera = m_rig.headCamera;
                Camera.tag = "MainCamera";
            }
            else{
                Camera = Camera.main;
                Camera.transform.SetParent(cachedTransform, false);
                Camera.transform.localPosition = UnityEngine.Vector3.zero;
                Camera.transform.localRotation = UnityEngine.Quaternion.Euler(0, 180f, 0);
            }
        }

        IEnumerator DisableVRRoutine()
        {
            XRSettings.LoadDeviceByName("");
            XRSettings.enabled = false;
            yield return null;
            //Camera.main.ResetAspect();
        }

        IEnumerator EnableVRRoutine(string deviceName)
        {
            if (deviceName == "None")
                yield break;

            // Todo: Figure out how to check for None, OpenVR, and Oculus without failing out.
            try{
                XRSettings.LoadDeviceByName(deviceName);
            } catch(Exception e){
                Debug.Log("Failed to find device: " + deviceName);
            }


            int tries = 5;
            while (true)
            {
                if (0 != string.Compare(XRSettings.loadedDeviceName, deviceName, true))
                {
                    tries--;
                    yield return null;
                }
                else
                {
                    Debug.Log("Enabled VR device: " + XRSettings.loadedDeviceName);
                    XRSettings.enabled = true;
                    yield break;
                }
            }
        }
    }
}