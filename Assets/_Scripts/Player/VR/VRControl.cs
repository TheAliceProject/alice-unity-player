using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;
using UnityEngine.XR;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Diagnostics;
#endif

public class VRControl : MonoBehaviour
{
    private static VRControl _instance;

    public const float INITIAL_CAMERA_ANGLE_CUTOFF = 20f; // Degrees. For desktop worlds, the initial angle is a good idea. In VR, worlds generally look better
                                                          // if the player is completely upright. We'll still let them turn the camera by large amounts, but filter out little (unintentional) ones
    public const float TRIGGER_SENSITIVITY = 0.5f;
    public const float WORLD_CANVAS_DISTANCE = 1.5f;
    public Toggle loadInVRToggle;
    public VRRig rig;
    public EventSystem eventSystem;
    public GameObject VRObjects;

    private string VRTypeFound = "";
    private bool loadWorldInVR = false;
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
            else{
                WorldObjects.GetVRObjects().SetActive(false);
            }
        }
#endif
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    void Update()
    {
        if(XRSettings.enabled)
        {
            CheckTriggers();
        }
    }
#endif

    public static void Loaded(bool value) {
        if (_instance != null) {
            _instance.loadWorldInVR = value;
            _instance.SetVROutput(value ? _instance.VRTypeFound : "");
        }
    }

    internal static bool IsLoadedInVR() {
        return _instance != null && _instance.loadWorldInVR;
    }

    internal static void SetRig(VRRig m_rig) {
        if (_instance != null) {
            _instance.rig = m_rig;
        }
    }

    internal static void EnablePointersForObjects(bool ena) {
        if (_instance != null && _instance.rig != null) {
            _instance.rig.EnablePointersForManipulation(ena);
        }
    }

    internal static VRRig Rig(){
        if (_instance != null){
            return _instance.rig;
        }
        return null;
    }

    internal static EventSystem EventSystem(){
        if (_instance != null){
            return _instance.eventSystem;
        }
        return null;
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
                VRObjects.SetActive(true);
                XRSettings.enabled = true;
                loadWorldInVR = true;
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

    public static Transform GetLastControllerClicked()
    {
        return _instance == null ? null : _instance.lastControllerClicked;
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

        if (!lastRightTrigger && Input.GetAxis("RightTrigger") >= TRIGGER_SENSITIVITY)
        {
            lastRightTrigger = true;
            rightTriggerDown = true;
            lastControllerClicked = rig.rightController;
        }
        else if (lastRightTrigger && Input.GetAxis("RightTrigger") < TRIGGER_SENSITIVITY)
        {
            lastRightTrigger = false;
            rightTriggerUp = true;
        }

        if (!lastLeftTrigger && Input.GetAxis("LeftTrigger") >= TRIGGER_SENSITIVITY)
        {
            lastLeftTrigger = true;
            leftTriggerDown = true;
            lastControllerClicked = rig.leftController;
        }
        else if (lastLeftTrigger && Input.GetAxis("LeftTrigger") < TRIGGER_SENSITIVITY)
        {
            lastLeftTrigger = false;
            leftTriggerUp = true;
        }

    }
    public static bool IsRightTriggerDown()
    {
        return _instance != null && _instance.rightTriggerDown;
    }

    public static bool IsRightTriggerUp()
    {
        return _instance != null && _instance.rightTriggerUp;
    }

    public static bool IsLeftTriggerDown()
    {
        return _instance != null && _instance.leftTriggerDown;
    }

    public static bool IsLeftTriggerUp()
    {
        return _instance != null && _instance.leftTriggerUp;
    }
}
