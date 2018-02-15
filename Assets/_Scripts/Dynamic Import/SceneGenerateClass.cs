using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGenerateClass : MonoBehaviour {

    [SerializeField] private string className;

	private void Start () {
        GenerateCodeDom.GenerateClass.Main(className);
    }
}
