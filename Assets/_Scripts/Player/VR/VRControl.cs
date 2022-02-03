using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeauRoutine;
using UnityEngine.XR;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;
#if !UNITY_WEBGL
using System.Diagnostics;
#endif

public class VRControl : MonoBehaviour
{
    public enum VRDevice{
        None,
        Vive,
        OculusRift,
        OculusS,
        OculusGo,
        OculusQuest,
        WindowsMR
    }

    private static VRControl _instance;

    public const float INITIAL_CAMERA_ANGLE_CUTOFF = 20f; // Degrees. For desktop worlds, the initial angle is a good idea. In VR, worlds generally look better
                                                          // if the player is completely upright. We'll still let them turn the camera by large amounts, but filter out little (unintentional) ones
    public const float TRIGGER_SENSITIVITY = 0.5f;
    public const float WORLD_CANVAS_DISTANCE = 1.5f;
    public VRRig rig;
    public Transform landingRig;
    public EventSystem eventSystem;
    public GameObject VRObjects;

    private string VRTypeFound = "";
    private VRDevice deviceType = VRDevice.None;
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
        // If target framerate is unconstrained, graphics card will sometimes scream 
        // while trying to go as fast as possible. 100 FPS should be plenty.
        Application.targetFrameRate = 100;
#if !UNITY_WEBGL
        SelectVRSystem(CurrentVrSystem());
#endif
    }

#if !UNITY_WEBGL
    private VRDevice CurrentVrSystem() {
        if (SystemInfo.deviceName.Contains("Oculus Quest"))
            return VRDevice.OculusQuest;

        var devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        if (devices.Any(device => device.name.Contains("Oculus Rift")))
            return VRDevice.OculusRift;

        if (Process.GetProcessesByName("vrserver").Length > 0)
            return VRDevice.Vive;

        return VRDevice.None;
    }

    void Update()
    {
        if(XRSettings.enabled)
        {
            CheckTriggers();
        }
    }
#endif

    internal static VRDevice LoadedVRDevice(){
        if (_instance == null){
            return VRDevice.None;
        }
        return _instance.deviceType;
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

#if !UNITY_WEBGL
    private void SelectVRSystem(VRDevice device) {
        if (device == VRDevice.None) {
            XRSettings.enabled = false;
            WorldObjects.SetVRObjectsActive(false);
        } else
            m_routine.Replace(this, EnableVR(device));
    }

    private IEnumerator EnableVR(VRDevice device) {
        yield return null;
        deviceType = device;
        VRObjects.SetActive(true);
        loadWorldInVR = true;
        Application.targetFrameRate = -1; // Use VR framerate specified by SDK

        switch (deviceType) {
            // Vive appears to be backwards when using SteamVR. Correct it here so we have identical behavior to Oculus
            case VRDevice.Vive:
                landingRig.localRotation = 
                    Quaternion.Euler(landingRig.localRotation.eulerAngles + new Vector3(0f, 180f, 0f));
                break;
            case VRDevice.OculusRift:
            case VRDevice.OculusS:
            case VRDevice.OculusGo:
            case VRDevice.OculusQuest: {
                try {
                    var xrInputSubsystems = new List<XRInputSubsystem>();
                    xrInputSubsystems.Clear();
                    SubsystemManager.GetInstances(xrInputSubsystems);
                    foreach (var xri in xrInputSubsystems) {
                        xri.TrySetTrackingOriginMode(((TrackingOriginModeFlags?) TrackingOriginModeFlags.Floor).Value);
                    }
                }
                catch (Exception ex) {
                    Debug.LogError($"Unable to set TrackingOriginMode: {ex}");
                }
                break;
            }
        }
    }
#endif

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
        }
        else if (lastLeftTrigger && Input.GetAxis("LeftTrigger") < TRIGGER_SENSITIVITY)
        {
            lastLeftTrigger = false;
            leftTriggerUp = true;
        }
    }

    public static bool IsMenuTriggerDown() {
        // " (1) Sandwich button refers to the Vive menu button. This button is mapped to primaryButton,
        // rather than menuButton, in order to better handle cross-platform applications. "
        // See https://docs.unity3d.com/Manual/xr_input.html
        return Input.GetButtonDown(LoadedVRDevice() == VRDevice.Vive ? "PrimaryRight" : "MenuLeft");
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
