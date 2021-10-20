using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Parse;

public class WorldObjects : MonoBehaviour
{
    private static WorldObjects _instance;

    public GameObject introCanvas;
    public GameObject vrObjects;
    public UnityObjectParser parser;

    public static string SCENE_GRAPH_LIBRARY_NAME = "SceneGraphLibrary";
    public static string DEFAULT_BUNDLED_WORLD_NAME = "DefaultBundledWorld";
    public static string DEFAULT_FOLDER_PATH = "Default";
    
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

    internal static GameObject GetVRObjects()
    {
        if (_instance != null)
        {
            return _instance.vrObjects;
        }
        return null;
    }

    internal static WorldExecutionState GetWorldExecutionState() {
        return _instance != null ? _instance._executionState : null;
    }
}