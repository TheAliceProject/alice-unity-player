using UnityEngine;
using Alice.Tweedle.Parse;

public class WorldObjects : MonoBehaviour
{
    private static WorldObjects _instance;

    public GameObject introCanvas;
    public GameObject vrObjects;
    public UnityObjectParser parser;

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

    internal static UnityObjectParser GetParser(){
        if (_instance != null){
            return _instance.parser;
        }
        return null;
    }

    internal static void SetVRObjectsActive(bool isActive) {
        if (_instance != null) {
            _instance.vrObjects.SetActive(isActive);
        }
    }

    internal static WorldExecutionState GetWorldExecutionState() {
        return _instance != null ? _instance._executionState : null;
    }
}