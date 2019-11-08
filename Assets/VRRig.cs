﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class VRRig : MonoBehaviour
{
    public Transform head;
    public Transform leftController;
    public Transform rightController;
    public Transform canvasRoot;
    public Camera headCamera;
    public LineRenderer rightRend;
    public LineRenderer leftRend;

    private EventSystem eventSystem;
    private bool enabledForUI = false;
    private bool enabledForManipulation = false;
    private bool enabledForControl = false;

    private List<GameObject> selectedButtons = new List<GameObject>();

    void Awake(){
        leftRend.enabled = false;
        rightRend.enabled = false;
        enabledForManipulation = false;
        enabledForUI = false;
        enabledForControl = false;
        eventSystem = VRControl.EventSystem();
    }

    private void EnablePointers(bool ena){
        if(leftRend != null)
            leftRend.enabled = ena;
        if(rightRend != null)
            rightRend.enabled = ena;
    }

    public bool IsEnabledForUI(){
        return enabledForUI;
    }

    public void EnablePointersForUI(bool ena){
        EnablePointers(ena || enabledForManipulation || enabledForControl);
        enabledForUI = ena;
    }

    public void EnablePointersForControl(bool ena){
        EnablePointers(ena || enabledForManipulation || enabledForUI);
        enabledForControl = ena;
    }
    
    public void EnablePointersForManipulation(bool ena)
    {
        EnablePointers(ena);
        enabledForManipulation = ena;
    }

    void Update(){
        if(enabledForManipulation || enabledForUI || enabledForControl){
            float leftPointerDistance = 10f;
            float rightPointerDistance = 10f;
            if(enabledForUI || enabledForControl){
                leftPointerDistance = CheckHandRaycasts(leftController);
                rightPointerDistance = CheckHandRaycasts(rightController);
            }

            for (int i = 0; i < 2; i++){
                rightRend.SetPosition(i, rightController.position + (rightController.forward * ((float)i * rightPointerDistance)));
                leftRend.SetPosition(i, leftController.position + (leftController.forward * ((float)i * leftPointerDistance)));
            }

        }
    }

    private float CheckHandRaycasts(Transform controller){
        RaycastHit hit;
        float pointerDistance = VRControl.WORLD_CANVAS_DISTANCE;
        if (Physics.Raycast(controller.position, controller.forward, out hit, 5f)){
            pointerDistance = hit.distance;
            Button hitButton = hit.transform.gameObject.GetComponent<Button>();
            if (hitButton != null){
                ExecuteEvents.Execute(hitButton.gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
                selectedButtons.Add(hitButton.gameObject);

                if (VRControl.IsLeftTriggerUp() || VRControl.IsRightTriggerUp()){
                    ExecuteEvents.Execute(hitButton.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
                }
            }
        }
        if (selectedButtons.Count > 0)
        {
            for (int i = selectedButtons.Count - 1; i >= 0; i--)
            {
                ExecuteEvents.Execute(selectedButtons[i], new BaseEventData(eventSystem), ExecuteEvents.deselectHandler);
                selectedButtons.RemoveAt(i);
            }

        }

        return pointerDistance;
    }
}
