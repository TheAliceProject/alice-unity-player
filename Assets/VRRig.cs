using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRig : MonoBehaviour
{
    public Transform head;
    public Transform leftController;
    public Transform rightController;

    public Camera headCamera;
    private LineRenderer rightRend;
    private LineRenderer leftRend;
    private bool laserPointersEnabled = false;

    void Awake()
    {
        CreatePointers();
        leftRend.enabled = false;
        rightRend.enabled = false;
        laserPointersEnabled = false;
    }

    private void CreatePointers()
    {
        if(rightController != null){
            rightRend = rightController.gameObject.AddComponent<LineRenderer>();
            rightRend.material = new Material(Shader.Find("Sprites/Default"));
            rightRend.startColor = Color.white;
            rightRend.endColor = Color.clear;
            rightRend.startWidth = 0.01f;
            rightRend.positionCount = 2;
        }
        if(leftController != null){
            leftRend = leftController.gameObject.AddComponent<LineRenderer>();
            leftRend.material = new Material(Shader.Find("Sprites/Default"));
            leftRend.startColor = Color.white;
            leftRend.endColor = Color.clear;
            leftRend.startWidth = 0.01f;
            leftRend.positionCount = 2;
        }
    }

    public void EnableLaserPointers(bool ena)
    {
        if(leftRend != null)
            leftRend.enabled = ena;
        if(rightRend != null)
            rightRend.enabled = ena;
        laserPointersEnabled = ena;
    }

    void Update()
    {
        if(laserPointersEnabled)
        {   
            if(this == null || rightRend == null || leftRend == null){
                laserPointersEnabled = false;
                return;
            }
            rightRend.SetPosition(0, rightController.position);
            rightRend.SetPosition(1, rightController.position + (rightController.forward * 10f));

            leftRend.SetPosition(0, leftController.position);
            leftRend.SetPosition(1, leftController.position + (leftController.forward * 10f));
        }
    }
}
