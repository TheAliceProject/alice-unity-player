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

    private const float TRIGGER_SENSITIVITY = 0.95f;

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

    void Update()
    {
        if(Input.GetAxis("RightTrigger") > TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Right trigger pressed!");
        else if (Input.GetAxis("LeftTrigger") > TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Left trigger pressed!");
        else if (Input.GetButtonDown("RightGrip"))
            UnityEngine.Debug.Log("Right grip pressed!");
        else if (Input.GetButtonDown("LeftGrip"))
            UnityEngine.Debug.Log("Left grip pressed!");

        else if (Input.GetButtonDown("PrimaryRight"))
            UnityEngine.Debug.Log("Right primary pressed!");
        else if (Input.GetButtonDown("PrimaryLeft"))
            UnityEngine.Debug.Log("Left primary pressed!");
        else if (Input.GetButtonDown("SecondaryRight"))
            UnityEngine.Debug.Log("Right secondary pressed!");
        else if (Input.GetButtonDown("SecondaryLeft"))
            UnityEngine.Debug.Log("Left secondary pressed!");
        else if (Input.GetButtonDown("MenuRight"))
            UnityEngine.Debug.Log("Right menu pressed!");
        else if (Input.GetButtonDown("MenuLeft"))
            UnityEngine.Debug.Log("Left menu pressed!");

        else if (Input.GetAxis("RightThumbstickLeftRight") > TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Right thumbstick right!");
        else if (Input.GetAxis("RightThumbstickLeftRight") < -TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Right thumbstick left!");
        else if (Input.GetAxis("RightThumbstickUpDown") > TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Right thumbstick up!");
        else if (Input.GetAxis("RightThumbstickUpDown") < -TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Right thumbstick down!");
        else if(Input.GetButtonDown("RightThumbstickClick"))
            UnityEngine.Debug.Log("Right thumbstick clicked");

        else if (Input.GetAxis("LeftThumbstickLeftRight") > TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Left thumbstick right!");
        else if (Input.GetAxis("LeftThumbstickLeftRight") < -TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Left thumbstick left!");
        else if (Input.GetAxis("LeftThumbstickUpDown") > TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Left thumbstick up!");
        else if (Input.GetAxis("LeftThumbstickUpDown") < -TRIGGER_SENSITIVITY)
            UnityEngine.Debug.Log("Left thumbstick down!");
        else if (Input.GetButtonDown("LeftThumbstickClick"))
            UnityEngine.Debug.Log("Left thumbstick clicked");


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
