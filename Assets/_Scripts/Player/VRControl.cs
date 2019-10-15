using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;
using UnityEngine.XR;
using UnityEngine.UI;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Diagnostics;
#endif

public class VRControl : MonoBehaviour
{
    public bool LoadWorldInVR = false;
    public string VRTypeFound = "";

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
    }

    void Start()
    {
    // On mac, there won't be any VR support for now
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        Process[] pname = Process.GetProcessesByName("vrserver");
         if(pname != null && pname.Length > 0){
            VRTypeFound = "OpenVR";
            SetVROutput("OpenVR");
        } 
        else{
            pname = Process.GetProcessesByName("OculusClient");
            if (pname != null && pname.Length > 0){
                VRTypeFound = "Oculus";
                SetVROutput("Oculus");
            }
        }
#endif
    }

    public void SetVROutput(string deviceToLoad)
    {
        m_routine.Replace(this, SwitchOutput(deviceToLoad));
    }

    private IEnumerator SwitchOutput(string deviceToLoad)
    {
        if (deviceToLoad != "")
        {
            XRSettings.LoadDeviceByName(deviceToLoad);
            yield return null;
            
            if(XRSettings.loadedDeviceName != deviceToLoad)
            {
                UnityEngine.Debug.Log("Problem loading device: " + deviceToLoad);
                XRSettings.LoadDeviceByName("");
                XRSettings.enabled = false;
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
