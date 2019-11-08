using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Parse;

public class WorldObjects : MonoBehaviour
{
    private static WorldObjects _instance;

    public GameObject introCanvas;
    public UnityObjectParser parser;

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
}
