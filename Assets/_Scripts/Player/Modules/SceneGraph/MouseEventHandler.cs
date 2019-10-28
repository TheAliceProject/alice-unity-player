using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using UnityEngine.XR;

namespace Alice.Player.Unity 
{
    public class MouseEventHandler
    {
        private List<MouseEventListenerProxy> m_MouseClickListeners = new List<MouseEventListenerProxy>();

        private float lastMouseDownTime = 0f;
        private SGModel sgObject;
        private Transform objectToMove = null;
        private Plane movementPlane = new Plane(UnityEngine.Vector3.up, UnityEngine.Vector3.zero);
        private Plane verticalMovementPlane;
        private UnityEngine.Vector3 objectOriginPoint = UnityEngine.Vector3.zero;
        private float yClickOffset;
        private UnityEngine.Vector3 planeOriginPoint = UnityEngine.Vector3.zero;
        private float dragSpeed = 10f;
        private UnityEngine.Vector3 dragOrigin;
        private UnityEngine.Vector3 shiftOrigin;
        private UnityEngine.Vector3 rotateOrigin;
        private bool defaultModelManipulationActive = false;
        internal bool isMac;

        public void SetModelManipulation(bool active){
            defaultModelManipulationActive = active;
        }

        public void AddMouseListener(MouseEventListenerProxy listener){
            m_MouseClickListeners.Add(listener);
        }

        public void HandleMouseEvents(){
            if (IsMouseOrTriggerDown()){ // Left mouse click
                lastMouseDownTime = Time.time;
                RaycastHit hit;
                Ray ray = GetRayFromMouseOrController();
                if (Physics.Raycast(ray, out hit, 100.0f)){
                    if (defaultModelManipulationActive){
                        sgObject = hit.transform.GetComponentInParent<SGModel>();
                        objectToMove = sgObject.transform;
                        objectOriginPoint = hit.transform.position;

                        verticalMovementPlane = new Plane(UnityEngine.Vector3.forward, objectOriginPoint);
                        yClickOffset = hit.point.y - objectOriginPoint.y;

                        // Use hit.point.y to base movement plane on mouse click, not model origin
                        UnityEngine.Vector3 clickOriginPoint = new UnityEngine.Vector3(objectOriginPoint.x, hit.point.y , objectOriginPoint.z);
                        movementPlane = new Plane(UnityEngine.Vector3.up, clickOriginPoint);
                        float distance;
                        if (movementPlane.Raycast(ray, out distance))
                            planeOriginPoint = ray.origin + (ray.direction * distance);
                    }
                }
            }

            if (IsVerticalModifierDown())
                shiftOrigin = Input.mousePosition;
            if (IsRotateModifierDown())
                rotateOrigin = Input.mousePosition;
            if (IsMouseOrTriggerUp()){
                objectToMove = null;
                if (Time.time - lastMouseDownTime < 0.25f){ // Considered a click and not a hold
                    RaycastHit hit;
                    Ray ray = GetRayFromMouseOrController();
                    Portion distanceFromLeft = XRSettings.enabled ? Portion.NONE : new Portion((float)Input.mousePosition.x / (float)Screen.width);
                    Portion distanceFromBottom = XRSettings.enabled ? Portion.NONE : new Portion((float)Input.mousePosition.y / (float)Screen.height);
                    for (int i = 0; i < m_MouseClickListeners.Count; i++){
                        if (m_MouseClickListeners[i].onlyOnModels){ // Clicked on object event
                            if (Physics.Raycast(ray, out hit, 100.0f)){
                                if (m_MouseClickListeners[i].targets.Length == 0){ // They didn't specify visuals, so call event because we hit something
                                    m_MouseClickListeners[i].CallEvent(distanceFromBottom, distanceFromLeft, hit.transform.GetComponentInParent<SGModel>().owner);
                                }
                                else{  // Make sure what we clicked on is in the list of visuals
                                    for (int j = 0; j < m_MouseClickListeners[i].targets.Length; j++){
                                        if (m_MouseClickListeners[i].targets[j] == hit.transform.GetComponentInParent<SGModel>().transform.gameObject){
                                            m_MouseClickListeners[i].CallEvent(distanceFromBottom, distanceFromLeft, hit.transform.GetComponentInParent<SGModel>().owner);
                                        }
                                    }
                                }
                            }
                        }
                        else{ // Clicked on screen event
                            m_MouseClickListeners[i].CallEvent(distanceFromBottom, distanceFromLeft);
                        }
                    }
                }
            }


            if (defaultModelManipulationActive && (objectToMove != null) && (IsVerticalModifierUp() || IsRotateModifierUp())){
                objectOriginPoint = objectToMove.position;
                Ray planeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance;
                if (movementPlane.Raycast(planeRay, out distance))
                    planeOriginPoint = planeRay.origin + (planeRay.direction * distance);
            }

            if (IsMouseOrTriggerDown()){
                if(XRSettings.enabled){
                    Ray planeRay = GetRayFromMouseOrController();
                    float distance;
                    if (movementPlane.Raycast(planeRay, out distance))
                        dragOrigin = planeRay.origin + (planeRay.direction * distance);
                }
                else {
                    dragOrigin = Input.mousePosition;
                }

                return;
            }

            // After this point do nothing if mouse button is not held
            if(XRSettings.enabled){
                if(Input.GetAxis("RightTrigger") < VRControl.TRIGGER_SENSITIVITY && Input.GetAxis("LeftTrigger") < VRControl.TRIGGER_SENSITIVITY)
                    return;
            }
            else{
                if (!Input.GetMouseButton(0))
                    return; 
            }



            if (objectToMove == null && defaultModelManipulationActive){
                objectToMove = Camera.main.transform.parent;
            }

            if (objectToMove == Camera.main.transform.parent){ // Moving the camera
                if (IsVerticalModifierHeld()){
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - shiftOrigin);
                    UnityEngine.Vector3 move = new UnityEngine.Vector3(pos.x * dragSpeed, dragSpeed * pos.y, pos.y * dragSpeed);
                    objectToMove.position += move;
                    shiftOrigin = Input.mousePosition;
                }
                else if (IsRotateModifierHeld()){
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - rotateOrigin);
                    objectToMove.Rotate(-dragSpeed * pos.y * 4f, -dragSpeed * pos.x * 4f, 0);
                    rotateOrigin = Input.mousePosition;
                }
                else{   // Scroll
                    if(XRSettings.enabled)
                    {
                        Ray planeRay = GetRayFromMouseOrController();
                        float distance;
                        if (movementPlane.Raycast(planeRay, out distance))
                        {
                            UnityEngine.Vector3 pointalongplane = planeRay.origin + (planeRay.direction * distance);
                            UnityEngine.Vector3 moveAmount = (dragOrigin - pointalongplane).normalized / 20f;
                            dragOrigin -= moveAmount;
                            objectToMove.position = new UnityEngine.Vector3(objectToMove.position.x - moveAmount.x, objectToMove.position.y, objectToMove.position.z - moveAmount.z);
                        }
                    }
                    else
                    {
                        UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                        UnityEngine.Vector3 move = new UnityEngine.Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
                        objectToMove.position -= move;
                        dragOrigin = Input.mousePosition;     
                    }
                }
            }
            else if (objectToMove != null){ // Moving an object
                if (IsVerticalModifierHeld()) {
                    Ray planeRay = GetRayFromMouseOrController();
                    float distance;
                    if (verticalMovementPlane.Raycast(planeRay, out distance)) {
                        UnityEngine.Vector3 pointalongplane = planeRay.origin + (planeRay.direction * distance);
                        var vp = VantagePoint.FromUnity(
                            new UnityEngine.Vector3(objectOriginPoint.x, pointalongplane.y - yClickOffset, objectOriginPoint.z),
                            objectToMove.rotation);
                        sgObject.UpdateVantagePointProperty(vp);
                    }
                    shiftOrigin = Input.mousePosition;
                }
                else if (IsRotateModifierHeld()){
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - rotateOrigin);
                    objectToMove.Rotate(UnityEngine.Vector3.up, -dragSpeed * pos.x * 200f);
                    var vp = VantagePoint.FromUnity(objectToMove.position, objectToMove.rotation);
                    sgObject.UpdateVantagePointProperty(vp);
                    rotateOrigin = Input.mousePosition;
                }
                else{ // move object along plane
                    Ray planeRay = GetRayFromMouseOrController();
                    float distance;
                    if (movementPlane.Raycast(planeRay, out distance)){
                        UnityEngine.Vector3 pointalongplane = planeRay.origin + (planeRay.direction * distance);
                        UnityEngine.Vector3 moveAmount = planeOriginPoint - pointalongplane;
                        var vp = VantagePoint.FromUnity(
                            new UnityEngine.Vector3(objectOriginPoint.x - moveAmount.x, objectToMove.position.y, objectOriginPoint.z - moveAmount.z),
                            objectToMove.rotation);
                        sgObject.UpdateVantagePointProperty(vp);
                    }
                }

            }
        }

        private Ray GetRayFromMouseOrController()
        {
            Ray ray;
            Transform lastControllerClicked = VRControl.I.GetLastControllerClicked();
            if (XRSettings.enabled && lastControllerClicked != null){
                // Draw ray from controller forward
                ray = new Ray(lastControllerClicked.position, lastControllerClicked.forward);
            }
            else            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Draw ray from screen to mouse click point
            }
            return ray;
        }


        private bool IsMouseOrTriggerDown(){
            return Input.GetKeyDown(KeyCode.Mouse0) || VRControl.I.IsRightTriggerDown() || VRControl.I.IsLeftTriggerDown();
        }
        private bool IsMouseOrTriggerUp(){
            return Input.GetKeyUp(KeyCode.Mouse0) || VRControl.I.IsRightTriggerUp() || VRControl.I.IsLeftTriggerUp();
        }
        private bool IsVerticalModifierHeld(){
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }
        private bool IsVerticalModifierDown(){
            return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        }
        private bool IsRotateModifierHeld() {
            return
                (isMac && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))) ||
                (!isMac && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)));
        }
        private bool IsRotateModifierDown(){
            return
                (isMac && (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))) ||
                (!isMac && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)));
        }
        private bool IsVerticalModifierUp(){
            return Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift);
        }
        private bool IsRotateModifierUp(){
            return
                (isMac && (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))) ||
                (!isMac && (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)));
        }


    }
}
