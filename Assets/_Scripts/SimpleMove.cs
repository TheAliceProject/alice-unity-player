
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SimpleMove : MonoBehaviour
{
    public Transform camRig;
    public GameObject objToFollowForward;
    public SteamVR_TrackedObject trackedObj;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        float xPos = device.GetAxis().x;
        float yPos = device.GetAxis().y;

        Vector2 touchPosition = device.GetAxis();

        /* 
        if (touchPosition.y > 0.01f)
            camRig.transform.position += new Vector3(objToFollowForward.transform.forward.x * 0.01f, 0f, objToFollowForward.transform.forward.z * 0.03f);
        else if(touchPosition.y < -0.01f)
            camRig.transform.position -= new Vector3(objToFollowForward.transform.forward.x * 0.01f, 0f, objToFollowForward.transform.forward.z * 0.03f);

        if (touchPosition.x > 0.1f)
            camRig.transform.position += new Vector3(objToFollowForward.transform.right.x * 0.01f, 0f, objToFollowForward.transform.right.z * 0.03f);
        else if (touchPosition.x < -0.1f)
            camRig.transform.position -= new Vector3(objToFollowForward.transform.right.x * 0.01f, 0f, objToFollowForward.transform.right.z * 0.03f);
        */

        if (touchPosition.y != 0f)
            camRig.transform.position += new Vector3(objToFollowForward.transform.forward.x * touchPosition.y * 0.02f, 0f, objToFollowForward.transform.forward.z * touchPosition.y * 0.02f);
      
        if (touchPosition.x !=  0f)
            camRig.transform.position += new Vector3(objToFollowForward.transform.right.x * touchPosition.x * 0.02f, 0f, objToFollowForward.transform.right.z * touchPosition.x * 0.02f);
     }
}
