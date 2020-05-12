using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Vector3 startPosition;
    private bool ControllerIsIn = false;
    private bool InTheHand = false;
    private GameObject controller;
    private bool left = false;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(ControllerIsIn && !left && VRControl.IsRightTriggerUp())
        {
            InTheHand = true;
            ControllerIsIn = false;
        }

        else if(InTheHand && !left && VRControl.IsRightTriggerUp())
        {
            InTheHand = false;
            transform.position = startPosition;
        }

        if (ControllerIsIn && left && VRControl.IsLeftTriggerUp())
        {
            InTheHand = true;
            ControllerIsIn = false;
        }

        else if (InTheHand && left && VRControl.IsLeftTriggerUp())
        {
            InTheHand = false;
            transform.position = startPosition;
        }

        if (InTheHand)
            transform.position = controller.transform.position;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (InTheHand)
            return;

        if (collision.transform.name.Contains("Controller (right)"))
        {
            if(!InTheHand)
                ControllerIsIn = true;
            left = false;
            controller = collision.gameObject;
        }

        if(collision.transform.name.Contains("Controller (left)")){
            if (!InTheHand)
                ControllerIsIn = true;
            left = true;
            controller = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ControllerIsIn = false;
        controller = null;
    }
}
