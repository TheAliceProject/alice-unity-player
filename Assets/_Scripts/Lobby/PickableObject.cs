using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Vector3 startPosition;
    private bool ControllerIsIn = false;
    private bool InTheHand = false;
    private GameObject controller;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(ControllerIsIn && VRControl.IsRightTriggerUp())
        {
            InTheHand = true;
            ControllerIsIn = false;
        }

        else if(InTheHand && VRControl.IsRightTriggerUp())
        {
            InTheHand = false;
            transform.position = startPosition;
        }

        if (InTheHand)
            transform.position = controller.transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.name.Contains("Controller (right)"))
        {
            if(!InTheHand)
                ControllerIsIn = true;
            controller = collision.gameObject;
        }
    }
}
