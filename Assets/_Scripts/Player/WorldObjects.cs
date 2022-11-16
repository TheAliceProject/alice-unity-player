using UnityEngine;
using Alice.Tweedle.Parse;
using UnityEngine.Serialization;

public class WorldObjects : MonoBehaviour
{
    private static WorldObjects _instance;

    public GameObject introCanvas;
    public GameObject vrObjects;
    public GameController controller;

    public static readonly string ProjectExt = "a3w";
    public static readonly string ProjectPattern = $"*.{ProjectExt}";
    public static readonly string SceneGraphLibraryName = $"SceneGraphLibrary.{ProjectExt}";
    public static readonly string DefaultBundledWorldName = $"DefaultBundledWorld.{ProjectExt}";
    public static readonly string DefaultFolderPath = "Default";
    
    private readonly WorldExecutionState _executionState = new WorldExecutionState();
    
    void Awake()
    {
        if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }

    internal static GameObject GetIntroCanvas(){
        if (_instance != null){
            return _instance.introCanvas;
        }
        return null;
    }

    internal static GameController GetGameController(){
        if (_instance != null){
            return _instance.controller;
        }
        return null;
    }

    internal static void SetVRObjectsActive(bool isActive) {
        if (_instance != null) {
            _instance.vrObjects.SetActive(isActive);
        }
    }

    public static void DisableDesktopCanvas() {
        GetGameController().desktopCanvas.gameObject.SetActive(false);
    }

    internal static WorldExecutionState GetWorldExecutionState() {
        return _instance != null ? _instance._executionState : null;
    }
}