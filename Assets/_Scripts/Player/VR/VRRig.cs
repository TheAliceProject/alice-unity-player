using System.Collections;
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
    public GameObject cursorPrefab;

    private EventSystem eventSystem;
    private bool enabledForUI = false;
    private bool enabledForManipulation = false;
    private bool enabledForControl = false;

    private GameObject cursor;

    private List<GameObject> selectedButtons = new List<GameObject>();

    void Awake(){
        leftRend.enabled = false;
        rightRend.enabled = false;
        enabledForManipulation = false;
        enabledForUI = false;
        enabledForControl = false;
        eventSystem = VRControl.EventSystem();
    }

    private void Start()
    {
        cursor = Instantiate(cursorPrefab, transform);
        cursor.SetActive(false);
    }

    private void EnablePointers(bool ena){
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
        if (enabledForManipulation || enabledForUI || enabledForControl)
        {
            float rightPointerDistance = 10f;
            if (enabledForUI || enabledForControl)
            {
                rightPointerDistance = CheckHandRaycasts(rightController);
            }

            RaycastHit hit;
            Ray ray = new Ray(rightController.position, rightController.forward);
            float rayLength = 0;
            cursor.SetActive(false);
            while (Physics.Raycast(ray, out hit, rightPointerDistance))
            {
                Debug.Log(hit.transform.name);
                rayLength += hit.distance;
                if (rayLength >= rightPointerDistance)
                {
                    break;
                }
                // go through transparent objects
                if (hit.transform.gameObject.GetComponent<MeshRenderer>() == null
                    || hit.transform.gameObject.GetComponent<MeshRenderer>().enabled == false)
                {
                    ray = new Ray(hit.point + rightController.forward * 0.01f, rightController.forward);
                }
                // opaque or semitransparent
                else
                {
                    cursor.SetActive(true);
                    rightPointerDistance = rayLength;
                    cursor.transform.position = rightController.position + rightController.forward * rightPointerDistance;
                    break;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                rightRend.SetPosition(i, rightController.position + (rightController.forward * ((float)i * rightPointerDistance)));
            }
        }
    }

    private float CheckHandRaycasts(Transform controller){
        RaycastHit hit;
        float pointerDistance = VRControl.WORLD_CANVAS_DISTANCE;
        if (Physics.Raycast(controller.position, controller.forward, out hit, 500f)){
            pointerDistance = hit.distance;
            Button hitButton = hit.transform.gameObject.GetComponent<Button>();
            if (hitButton != null){
                ExecuteEvents.Execute(hitButton.gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
                selectedButtons.Add(hitButton.gameObject);

                if (VRControl.IsRightTriggerUp()){
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
