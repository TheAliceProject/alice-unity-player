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
            float currYValue = 0f;
            if (currRigPrefab != null)
            {
                currYValue = currRigPrefab.transform.position.y;
                Destroy(currRigPrefab);
            }
            currRigPrefab = Instantiate(CameraRigPrefab);
            Vector3 currRigPosition = currRigPrefab.transform.position;
            currRigPrefab.transform.position = new Vector3(currRigPosition.x, currYValue, currRigPosition.z);
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
