using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using UnityEngine.XR;

namespace Alice.Player.Unity 
{
    public class KeyboardEventHandler
    {
        private List<KeyEventListenerProxy> m_KeyPressListeners = new List<KeyEventListenerProxy>();
        private List<KeyCode> m_heldKeys = new List<KeyCode>();
        private List<string> m_heldKeyCodes = new List<string>();
        private ObjectKeyboardMover m_objectMover = new ObjectKeyboardMover();
        private List<KeyCode> m_keysToRemove = new List<KeyCode>();
        private List<string> m_vrKeysToRemove = new List<string>();

        public void AddListener(KeyEventListenerProxy listener){
            m_KeyPressListeners.Add(listener);
        }

        public void AddObjectMover(Transform objectToMove){
            m_objectMover.AddMover(objectToMove);
        }

        public void RemoveAllKeys(){
            m_heldKeys.Clear();
            m_heldKeyCodes.Clear();
            m_objectMover.ClearHeldKeys();
        }

        public void HandleKeyboardEvents(){
            if(m_KeyPressListeners.Count > 0 || m_objectMover.GetNumMovers() > 0){
                // Check for key down
                if(Input.anyKeyDown){
                    foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
                        if(Input.GetKeyDown(vKey)){
                            m_heldKeys.Add(vKey);
                            if(KeyMap.TweedleKeyLookup.ContainsKey(vKey))
                                NotifyKeyboardEvents((int)KeyMap.TweedleKeyLookup[vKey], true);
                            //ToDo: Would be nice to not need to check every key here. But we want to support more than one key press at a time, so no break;
                        }
                    }

                    // Check for VR Buttons
                    if(XRSettings.enabled){
                        foreach (string buttonString in KeyMap.VRButtonStrings){
                            if (Input.GetButtonDown(buttonString))
                            {
                                Key vrKey = KeyMap.VRButtonLookup[buttonString];
                                m_heldKeyCodes.Add(buttonString);
                                NotifyKeyboardEvents((int)vrKey, true);
                            }
                        }
                    }
                }

                // Check for VR Axes
                // TODO: Verify functionality with overlapping key policy
                if(XRSettings.enabled){
                    foreach (string axisString in KeyMap.VRAxisStrings){
                        if (Input.GetAxis(axisString) > VRControl.TRIGGER_SENSITIVITY){
                            string posString = axisString + "_P";
                            if (!m_heldKeyCodes.Contains(posString)){
                                m_heldKeyCodes.Add(posString);
                                NotifyKeyboardEvents((int)KeyMap.VRAxisLookup[posString], true);
                            }
                        }
                        else if (Input.GetAxis(axisString) < -VRControl.TRIGGER_SENSITIVITY){
                            string negString = axisString + "_N";
                            if (!m_heldKeyCodes.Contains(negString)){
                                m_heldKeyCodes.Add(negString);
                                NotifyKeyboardEvents((int)KeyMap.VRAxisLookup[negString], true);
                            }
                        }
                    }

                    if (VRControl.I.IsLeftTriggerDown()){
                        m_heldKeyCodes.Add("LeftTrigger");
                        NotifyKeyboardEvents((int)Key.LEFT_TRIGGER, true);
                    }
                    if (VRControl.I.IsRightTriggerDown()){
                        m_heldKeyCodes.Add("RightTrigger");
                        NotifyKeyboardEvents((int)Key.RIGHT_TRIGGER, true);
                    }
                }


                // Check for key up if we've pressed one down in the past
                if(m_heldKeys.Count > 0){
                    m_keysToRemove.Clear();
                    foreach(KeyCode vKey in m_heldKeys){
                        if(Input.GetKeyUp(vKey)){
                            m_keysToRemove.Add(vKey);
                            if(KeyMap.TweedleKeyLookup.ContainsKey(vKey))
                                NotifyKeyboardEvents((int)KeyMap.TweedleKeyLookup[vKey], false);
                        }
                    }

                    // Remove keys from held key list, but not while we're iterating over said list
                    foreach(KeyCode vKey in m_keysToRemove){
                        m_heldKeys.Remove(vKey);
                    }
                    m_keysToRemove.Clear();
                }

                if (XRSettings.enabled){
                    if (m_heldKeyCodes.Count > 0){
                        m_vrKeysToRemove.Clear();
                        foreach (string vrKey in m_heldKeyCodes){
                            if (vrKey.Substring(vrKey.Length - 2) == "_P"){
                                if (Input.GetAxis(vrKey.Substring(0, vrKey.Length - 2)) < VRControl.TRIGGER_SENSITIVITY){
                                    m_vrKeysToRemove.Add(vrKey);
                                    NotifyKeyboardEvents((int)KeyMap.VRAxisLookup[vrKey], false);
                                }
                            }
                            else if (vrKey.Substring(vrKey.Length - 2) == "_N"){
                                if (Input.GetAxis(vrKey.Substring(0, vrKey.Length - 2)) > -VRControl.TRIGGER_SENSITIVITY){
                                    m_vrKeysToRemove.Add(vrKey);
                                    NotifyKeyboardEvents((int)KeyMap.VRAxisLookup[vrKey], false);
                                }
                            }
                            else{
                                // Button
                                if (Input.GetButtonUp(vrKey)){
                                    m_vrKeysToRemove.Add(vrKey);
                                    NotifyKeyboardEvents((int)KeyMap.VRButtonLookup[vrKey], false);
                                }
                                if(KeyMap.VRButtonLookup[vrKey] == Key.LEFT_TRIGGER && VRControl.I.IsLeftTriggerUp()){
                                    m_vrKeysToRemove.Add(vrKey);
                                    NotifyKeyboardEvents((int)Key.LEFT_TRIGGER, false);
                                }
                                if (KeyMap.VRButtonLookup[vrKey] == Key.RIGHT_TRIGGER && VRControl.I.IsRightTriggerUp()){
                                    m_vrKeysToRemove.Add(vrKey);
                                    NotifyKeyboardEvents((int)Key.RIGHT_TRIGGER, false);
                                }
                            }
                        }

                        foreach (string vKey in m_vrKeysToRemove){
                            m_heldKeyCodes.Remove(vKey);
                        }
                        m_vrKeysToRemove.Clear();
                    }
                }
            }
        }

        private void NotifyKeyboardEvents(int theKey, bool keyDown){
            // Notify keypress listeners
            for(int i = 0; i < m_KeyPressListeners.Count; i++){
                m_KeyPressListeners[i].NotifyEvent(theKey, keyDown);
            }
            // Notify object movers
            for (int i = 0; i < m_objectMover.GetNumMovers(); i++){
                m_objectMover.NotifyEvent(theKey, keyDown);
            }

        }
    }
}
