
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SimpleMove : MonoBehaviour
{
    public Transform camRig;
    public GameObject objToFollowForward;
    public SteamVR_TrackedObject trackedObj;
    public GameObject model;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        model.SetActive(CameraType.CamType == 0);
        if(CameraType.CamType == 1)
        {
            return;
        }
            

        var device = SteamVR_Controller.Input((int)trackedObj.index);

        float xPos = device.GetAxis().x;
        float yPos = device.GetAxis().y;

        Vector2 touchPosition = device.GetAxis();

        if (touchPosition.y != 0f)
            camRig.transform.position += new Vector3(objToFollowForward.transform.forward.x * touchPosition.y * 0.02f, 0f, objToFollowForward.transform.forward.z * touchPosition.y * 0.02f);
      
        if (touchPosition.x !=  0f)
            camRig.transform.position += new Vector3(objToFollowForward.transform.right.x * touchPosition.x * 0.02f, 0f, objToFollowForward.transform.right.z * touchPosition.x * 0.02f);
     }
}
