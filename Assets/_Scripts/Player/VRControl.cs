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
    private static VRControl _instance;
    public static VRControl I { get { return _instance; } }

    public const float INITIAL_CAMERA_ANGLE_CUTOFF = 20f; // Degrees. For desktop worlds, the initial angle is a good idea. In VR, worlds generally look better
                                                          // if the player is completely upright. We'll still let them turn the camera by large amounts, but filter out little (unintentional) ones
    public const float TRIGGER_SENSITIVITY = 0.95f;
    public bool LoadWorldInVR = false;
    public string VRTypeFound = "";
    public Toggle loadInVRToggle;
    public VRRig rig;

    private Routine m_routine;
    private bool lastRightTrigger = false;
    private bool lastLeftTrigger = false;

    private bool rightTriggerDown = false;
    private bool leftTriggerDown = false;
    private bool rightTriggerUp = false;
    private bool leftTriggerUp = false;
    private Transform lastControllerClicked = null;


    // Start is called before the first frame update
    void Awake()
    {
        rig = null;
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

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    void Update()
    {
        if(XRSettings.enabled)
            CheckTriggers();
    }
#endif

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

    public Transform GetLastControllerClicked()
    {
        return lastControllerClicked;
    }

    private void CheckTriggers()
    {
        // Make these act as buttons ( only active for one frame )
        if(rightTriggerDown)
            rightTriggerDown = false;
        if(leftTriggerDown)
            leftTriggerDown = false;
        if(rightTriggerUp)
            rightTriggerUp = false;
        if(leftTriggerUp)
            leftTriggerUp = false;

        if (!lastRightTrigger && Input.GetAxis("RightTrigger") >= VRControl.TRIGGER_SENSITIVITY)
        {
            lastRightTrigger = true;
            rightTriggerDown = true;
            lastControllerClicked = VRControl.I.rig.rightController;
        }
        else if (lastRightTrigger && Input.GetAxis("RightTrigger") < VRControl.TRIGGER_SENSITIVITY)
        {
            lastRightTrigger = false;
            rightTriggerUp = true;
        }

        if (!lastLeftTrigger && Input.GetAxis("LeftTrigger") >= VRControl.TRIGGER_SENSITIVITY)
        {
            lastLeftTrigger = true;
            leftTriggerDown = true;
            lastControllerClicked = VRControl.I.rig.leftController;
        }
        else if (lastLeftTrigger && Input.GetAxis("LeftTrigger") < VRControl.TRIGGER_SENSITIVITY)
        {
            lastLeftTrigger = false;
            leftTriggerUp = true;
        }

    }
    public bool RightTriggerDown()
    {
        return rightTriggerDown;
    }

    public bool RightTriggerUp()
    {
        return rightTriggerUp;
    }

    public bool LeftTriggerDown()
    {
        return leftTriggerDown;
    }

    public bool LeftTriggerUp()
    {
        return leftTriggerUp;
    }
}
