using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;
using UnityEngine.XR;
using UnityEngine.UI;

public class VRControl : MonoBehaviour
{
    public bool VRDeviceActive = false;
    public bool LoadWorldInVR = false;
    public string VRDeviceModel = "";

    public Toggle loadInVRToggle;
    private static VRControl _instance;
    public static VRControl I { get { return _instance; } }
    private Routine m_routine;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }

        m_routine.Replace(this, CheckVRStatus());
    }

    void Start()
    {
        SetVROutput(true);
    }

    public void SetVROutput(bool enable)
    {
        Routine.Start(SwitchOutput(enable));
    }

    private IEnumerator CheckVRStatus()
    {
        while(true)
        {
            if (XRDevice.isPresent)
            {
                VRDeviceActive = true;
                Debug.Log("Found model: " + XRDevice.model);
            }
            else
            {
                VRDeviceActive = false;
            }
            yield return 1f;
        }
    }

    private IEnumerator SwitchOutput(bool enableVR)
    {
        if (enableVR)
        {
            XRSettings.LoadDeviceByName("Oculus");
            yield return null;
            
            if(XRSettings.loadedDeviceName != "Oculus")
            {
                XRSettings.LoadDeviceByName("OpenVR");
                yield return null;
                if(XRSettings.loadedDeviceName == "OpenVR")
                {
                    XRSettings.enabled = true;
                    LoadWorldInVR = true;
                    loadInVRToggle.gameObject.SetActive(true);
                }
            }
            else
            {
                XRSettings.enabled = true;
                LoadWorldInVR = true;
                loadInVRToggle.gameObject.SetActive(true);
            }
        }
        else
        {
            XRSettings.LoadDeviceByName("");
            yield return null;
            XRSettings.enabled = false;
        }
    }
}
