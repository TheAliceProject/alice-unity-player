using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;

namespace Alice.Player.Unity 
{
    public class MouseEventHandler
    {
        private List<MouseEventListenerProxy> m_MouseClickListeners = new List<MouseEventListenerProxy>();

        private float lastMouseDownTime = 0f;
        private Transform objectToMove = null;
        private Plane movementPlane = new Plane(UnityEngine.Vector3.up, UnityEngine.Vector3.zero);
        private UnityEngine.Vector3 objectOriginPoint = UnityEngine.Vector3.zero;
        private UnityEngine.Vector3 planeOriginPoint = UnityEngine.Vector3.zero;
        private float dragSpeed = 10f;
        private UnityEngine.Vector3 dragOrigin;
        private UnityEngine.Vector3 shiftOrigin;
        private UnityEngine.Vector3 rotateOrigin;
        private bool defaultModelManipulationActive = false;

        public void SetModelManipulation(bool active){
            defaultModelManipulationActive = active;
        }

        public void AddMouseListener(MouseEventListenerProxy listener){
            m_MouseClickListeners.Add(listener);
        }

        public void HandleMouseEvents(){
            if (Input.GetKeyDown(KeyCode.Mouse0)){ // Left mouse click
                lastMouseDownTime = Time.time;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Draw ray from screen to mouse click point
                if (Physics.Raycast(ray, out hit, 100.0f)){
                    if (defaultModelManipulationActive){
                        objectToMove = hit.transform.GetComponentInParent<SGModel>().transform;  // transform.parent;
                        objectOriginPoint = hit.transform.position;
                        float distance;
                        if (movementPlane.Raycast(ray, out distance))
                            planeOriginPoint = ray.origin + (ray.direction * distance);
                    }
                }
            }

            if (IsShiftDown())
                shiftOrigin = Input.mousePosition;
            if (IsCtrlDown())
                rotateOrigin = Input.mousePosition;
            if (Input.GetKeyUp(KeyCode.Mouse0)){
                objectToMove = null;
                if (Time.time - lastMouseDownTime < 0.25f){ // Considered a click and not a hold
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Draw ray from screen to mouse click point
                    Portion distanceFromLeft = new Portion((float)Input.mousePosition.x / (float)Screen.width);
                    Portion distanceFromBottom = new Portion((float)Input.mousePosition.y / (float)Screen.height);
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


            if (defaultModelManipulationActive && (objectToMove != null) && (IsShiftUp() || IsCtrlUp())){
                objectOriginPoint = objectToMove.position;
                Ray planeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance;
                if (movementPlane.Raycast(planeRay, out distance))
                    planeOriginPoint = planeRay.origin + (planeRay.direction * distance);
            }

            if (Input.GetMouseButtonDown(0)){
                dragOrigin = Input.mousePosition;
                return;
            }

            // After this point do nothing if mouse button is not held
            if (!Input.GetMouseButton(0))
                return;


            if (objectToMove == null && defaultModelManipulationActive){
                objectToMove = Camera.main.transform.parent;
            }

            if (objectToMove == Camera.main.transform.parent){ // Moving the camera
                if (IsShiftHeld()){    // Up down
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - shiftOrigin);
                    UnityEngine.Vector3 move = new UnityEngine.Vector3(pos.x * dragSpeed, dragSpeed * pos.y, pos.y * dragSpeed);
                    objectToMove.position += move;
                    shiftOrigin = Input.mousePosition;
                }
                else if (IsCtrlHeld()){ // Rotate
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - rotateOrigin);
                    objectToMove.Rotate(UnityEngine.Vector3.up, dragSpeed * pos.x * 20f);
                    rotateOrigin = Input.mousePosition;
                }
                else{   // Scroll
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                    UnityEngine.Vector3 move = new UnityEngine.Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
                    objectToMove.position += move;
                    dragOrigin = Input.mousePosition;
                }
            }
            else if (objectToMove != null){ // Moving an object
                // If holding shift, move object up and down
                if (IsShiftHeld()){
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - shiftOrigin);
                    UnityEngine.Vector3 move = new UnityEngine.Vector3(0f, dragSpeed * pos.y, 0f);
                    objectToMove.position += move;
                    shiftOrigin = Input.mousePosition;
                }
                else if (IsCtrlHeld()){ // If holding control, rotate object
                    UnityEngine.Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - rotateOrigin);
                    objectToMove.Rotate(UnityEngine.Vector3.up, dragSpeed * pos.x * 200f);
                    rotateOrigin = Input.mousePosition;
                }
                else{ // move object along plane
                    Ray planeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float distance;
                    if (movementPlane.Raycast(planeRay, out distance)){
                        UnityEngine.Vector3 pointalongplane = planeRay.origin + (planeRay.direction * distance);
                        UnityEngine.Vector3 moveAmount = planeOriginPoint - pointalongplane;
                        objectToMove.position = new UnityEngine.Vector3(objectOriginPoint.x - moveAmount.x, objectToMove.position.y, objectOriginPoint.z - moveAmount.z);
                    }
                }

            }
        }

        private bool IsShiftHeld(){
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }
        private bool IsShiftDown(){
            return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        }
        private bool IsCtrlHeld(){
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        }
        private bool IsCtrlDown(){
            return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
        }
        private bool IsShiftUp(){
            return Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift);
        }
        private bool IsCtrlUp(){
            return Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl);
        }
    }
}
