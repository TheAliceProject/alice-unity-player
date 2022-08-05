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
    public GameObject vrMenu;
    public GameObject cursorPrefab;

    private EventSystem eventSystem;
    private bool enabledForUI = false;
    private bool enabledForManipulation = false;
    private bool enabledForControl = false;

    private GameObject cursor;
    private Light controllerLight;

    private List<GameObject> selectedButtons = new List<GameObject>();

    void Awake(){
        leftRend.enabled = false;
        rightRend.enabled = false;
        enabledForManipulation = false;
        enabledForUI = false;
        enabledForControl = false;
        eventSystem = VRControl.EventSystem();
        CreateLight();
    }
    
    // A light on the left hand so the controls are visible and usable in all cases.
    private void CreateLight() {
        if (controllerLight != null) return;
        
        controllerLight = new GameObject("Controller Area Light").AddComponent<Light>();
        controllerLight.transform.parent = leftController;
        controllerLight.type = LightType.Point;
        controllerLight.intensity = 1.0F;
        controllerLight.enabled = false;
    }

    public void EnableControllerLight(bool on) {
        controllerLight.enabled = on;
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

            rightPointerDistance = GetPointerDistance(rightPointerDistance);

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

    private float GetPointerDistance(float rightPointerDistance)
    {
        RaycastHit hit;
        Ray ray = new Ray(rightController.position, rightController.forward);
        float rayLength = 0;
        cursor.SetActive(false);
        while (Physics.Raycast(ray, out hit, rightPointerDistance))
        {
            rayLength += hit.distance;
            if (rayLength >= rightPointerDistance)
            {
                return rightPointerDistance;
            }
            // go through transparent objects
            if ((hit.transform.gameObject.GetComponent<MeshRenderer>() != null
                && hit.transform.gameObject.GetComponent<MeshRenderer>().enabled == false)
                || (hit.transform.gameObject.GetComponent<SkinnedMeshRenderer>() != null
                && hit.transform.gameObject.GetComponent<SkinnedMeshRenderer>().enabled == false))
            {
                ray = new Ray(hit.point + rightController.forward * 0.01f, rightController.forward);
            }
            // opaque or semitransparent
            else
            {
                cursor.SetActive(true);
                cursor.transform.position = rightController.position + rightController.forward * rayLength;
                return rayLength;
            }
        }
        return rightPointerDistance;
    }
}
