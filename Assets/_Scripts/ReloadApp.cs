using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadApp : MonoBehaviour
{
    public Button reloadButton;
    public GameObject introCanvas;
    public GameObject CameraRigPrefab;
    private GameObject currRigPrefab;

    void Start()
    {
        currRigPrefab = GameObject.Find("[CameraRig]");
        reloadButton.onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("SceneGraph"));
            introCanvas.gameObject.SetActive(true);
            if(currRigPrefab != null)
                Destroy(currRigPrefab);
            currRigPrefab = Instantiate(CameraRigPrefab);
        });
    }
}

public static class CameraType
{
    public static int CamType = 1;

    public static void SetCameraType(int type)
    {
        CamType = type;
    }
}
