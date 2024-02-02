using System.Collections;
using UnityEngine;
using BeauRoutine;
using Unity.XR.Oculus;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.XR.Management;
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
    public GameObject StartObjects;
    public GameObject HideObjects;

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
        SelectVRSystem();
#endif
    }

#if !UNITY_WEBGL
    private static VRDevice GetVRDevice() {
        return Process.GetProcessesByName("vrserver").Length > 0 ? VRDevice.Vive : GetOculusVRDevice();
    }

    private static VRDevice GetOculusVRDevice() {
        switch (Utils.GetSystemHeadsetType()) {
            // case SystemHeadset.Oculus_Go = 7 has been removed from the SystemHeadset enum
            case SystemHeadset.Rift_DK1: // Prototype 1 - 4096
            case SystemHeadset.Rift_DK2: // Prototype 2 - 4097
            case SystemHeadset.Rift_CV1: // Rift - 4098
            case SystemHeadset.Rift_CB: // 4099
                return VRDevice.OculusRift;
            case SystemHeadset.Rift_S: // Rift S 4100
            // Placeholder values after the Rift S, but not the linked Quests, which are below
            // TODO: revisit these as they become defined
            case SystemHeadset.PC_Placeholder_4103:
            case SystemHeadset.PC_Placeholder_4104:
            case SystemHeadset.PC_Placeholder_4105:
            case SystemHeadset.PC_Placeholder_4106:
            case SystemHeadset.PC_Placeholder_4107:
                return VRDevice.OculusS;
            case SystemHeadset.Oculus_Quest: // 8
            case SystemHeadset.Oculus_Quest_2: // 9
            case SystemHeadset.Placeholder_10:
            case SystemHeadset.Placeholder_11:
            case SystemHeadset.Placeholder_12:
            case SystemHeadset.Placeholder_13:
            case SystemHeadset.Placeholder_14:
            case SystemHeadset.Oculus_Link_Quest: // 4101 recognizing all Quest variants as Quest
            case SystemHeadset.Oculus_Link_Quest_2: // 4102
                return VRDevice.OculusQuest;
            case SystemHeadset.None:
            default:
                return VRDevice.None;
        }
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
    private void SelectVRSystem() {
        deviceType = GetVRDevice();
        if (deviceType == VRDevice.None) {
            WorldObjects.SetVRObjectsActive(false);
        } else {
            loadWorldInVR = true;
            WorldObjects.DisableDesktopCanvas();
            m_routine.Replace(this, EnableVR());
        }
    }

    private IEnumerator EnableVR()
    {
        yield return null;
        VRObjects.SetActive(true);
        StartObjects.SetActive(true);
        if (!WorldObjects.GetWorldExecutionState().IsMainMenuAllowed())
            HideObjects.SetActive(false);
        Application.targetFrameRate = -1; // Use VR framerate specified by SDK

        switch (deviceType)
        {
            // Vive appears to be backwards when using SteamVR. Correct it here so we have identical behavior to Oculus
            case VRDevice.Vive:
                landingRig.localRotation =
                    Quaternion.Euler(landingRig.localRotation.eulerAngles + new Vector3(0f, 180f, 0f));
                break;
            case VRDevice.OculusRift:
            case VRDevice.OculusS:
            case VRDevice.OculusGo:
            case VRDevice.OculusQuest:
            {
                var xrSettings = XRGeneralSettings.Instance;
                var xrManager = xrSettings.Manager;
                var xrLoader = xrManager.activeLoader;
                if (xrLoader == null) yield break;
                var xrInput = xrLoader.GetLoadedSubsystem<XRInputSubsystem>();
                xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
                break;
            }
        }
    }
#endif

    public static void HideControls() {
#if !UNITY_WEBGL
        if (_instance != null) {
            _instance.HideObjects.SetActive(false);
        }
#endif
    }

    public static void ShowControls() {
#if !UNITY_WEBGL
        if (_instance != null) {
            _instance.HideObjects.SetActive(true);
        }
#endif
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
