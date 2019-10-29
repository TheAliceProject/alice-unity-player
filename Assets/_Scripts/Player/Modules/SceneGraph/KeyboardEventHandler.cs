using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using UnityEngine.XR;
using BeauRoutine;

namespace Alice.Player.Unity 
{
    public class KeyboardEventHandler
    {
        
        public enum KeyAction{
            Press,
            Repeat,
            Release
        }

        private List<KeyEventListenerProxy> m_KeyPressListeners = new List<KeyEventListenerProxy>();
        private HashSet<KeyCode> m_heldKeys = new HashSet<KeyCode>();
        private List<string> m_heldKeyCodes = new List<string>();
        private ObjectKeyboardMover m_objectMover = new ObjectKeyboardMover();
        private List<KeyCode> m_keysToRemove = new List<KeyCode>();
        private List<string> m_vrKeysToRemove = new List<string>();

        private Routine m_repeat_key_routine;

        public void AddListener(KeyEventListenerProxy listener){
            m_KeyPressListeners.Add(listener);
            StartKeyRepeater();
        }

        public void AddObjectMover(SGTransformableEntity entity){
            m_objectMover.AddObject(entity);
            StartKeyRepeater();
        }

        public void RemoveAllKeys(){
            m_heldKeys.Clear();
            m_heldKeyCodes.Clear();
        }

        public void HandleKeyboardEvents() {
            if (m_KeyPressListeners.Count <= 0 && !m_objectMover.HasObjects()) {
                return;
            }
            // Check for key down
            if (Input.anyKeyDown) {
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKeyDown(vKey)) {
                        m_heldKeys.Add(vKey);
                        if (KeyMap.TweedleKeyLookup.ContainsKey(vKey))
                            NotifyKeyboardEvents(KeyMap.TweedleKeyLookup[vKey], KeyAction.Press);
                        //ToDo: Would be nice to not need to check every key here. But we want to support more than one key press at a time, so no break;
                    }
                }

                // Check for VR Buttons
                if (XRSettings.enabled) {
                    foreach (string buttonString in KeyMap.VRButtonStrings) {
                        if (Input.GetButtonDown(buttonString)) {
                            Key vrKey = KeyMap.VRButtonLookup[buttonString];
                            m_heldKeyCodes.Add(buttonString);
                            NotifyKeyboardEvents(vrKey, KeyAction.Press);
                        }
                    }
                }
            }

            // Check for VR Axes
            // TODO: Verify functionality with overlapping key policy
            if (XRSettings.enabled) {
                foreach (string axisString in KeyMap.VRAxisStrings) {
                    if (Input.GetAxis(axisString) > VRControl.TRIGGER_SENSITIVITY) {
                        string posString = axisString + "_P";
                        if (!m_heldKeyCodes.Contains(posString)) {
                            m_heldKeyCodes.Add(posString);
                            NotifyKeyboardEvents(KeyMap.VRAxisLookup[posString], KeyAction.Press);
                        }
                    } else if (Input.GetAxis(axisString) < -VRControl.TRIGGER_SENSITIVITY) {
                        string negString = axisString + "_N";
                        if (!m_heldKeyCodes.Contains(negString)) {
                            m_heldKeyCodes.Add(negString);
                            NotifyKeyboardEvents(KeyMap.VRAxisLookup[negString], KeyAction.Press);
                        }
                    }
                }

                if (VRControl.I.IsLeftTriggerDown()) {
                    m_heldKeyCodes.Add("LeftTrigger");
                    NotifyKeyboardEvents(Key.LEFT_TRIGGER, KeyAction.Press);
                }
                if (VRControl.I.IsRightTriggerDown()) {
                    m_heldKeyCodes.Add("RightTrigger");
                    NotifyKeyboardEvents(Key.RIGHT_TRIGGER, KeyAction.Press);
                }
            }


            // Check for key up if we've pressed one down in the past
            if (m_heldKeys.Count > 0) {
                m_keysToRemove.Clear();
                foreach (KeyCode vKey in m_heldKeys) {
                    if (Input.GetKeyUp(vKey)) {
                        m_keysToRemove.Add(vKey);
                        if (KeyMap.TweedleKeyLookup.ContainsKey(vKey))
                            NotifyKeyboardEvents(KeyMap.TweedleKeyLookup[vKey], KeyAction.Release);
                    }
                }

                // Remove keys from held key list, but not while we're iterating over said list
                foreach (KeyCode vKey in m_keysToRemove) {
                    m_heldKeys.Remove(vKey);
                }
                m_keysToRemove.Clear();
            }

            if (XRSettings.enabled) {
                if (m_heldKeyCodes.Count > 0) {
                    m_vrKeysToRemove.Clear();
                    foreach (string vrKey in m_heldKeyCodes) {
                        if (vrKey.Substring(vrKey.Length - 2) == "_P") {
                            if (Input.GetAxis(vrKey.Substring(0, vrKey.Length - 2)) < VRControl.TRIGGER_SENSITIVITY) {
                                m_vrKeysToRemove.Add(vrKey);
                                NotifyKeyboardEvents(KeyMap.VRAxisLookup[vrKey], KeyAction.Release);
                            }
                        } else if (vrKey.Substring(vrKey.Length - 2) == "_N") {
                            if (Input.GetAxis(vrKey.Substring(0, vrKey.Length - 2)) > -VRControl.TRIGGER_SENSITIVITY) {
                                m_vrKeysToRemove.Add(vrKey);
                                NotifyKeyboardEvents(KeyMap.VRAxisLookup[vrKey], KeyAction.Release);
                            }
                        } else {
                            // Button
                            if (Input.GetButtonUp(vrKey)) {
                                m_vrKeysToRemove.Add(vrKey);
                                NotifyKeyboardEvents(KeyMap.VRButtonLookup[vrKey], KeyAction.Release);
                            }
                            if (KeyMap.VRButtonLookup[vrKey] == Key.LEFT_TRIGGER && VRControl.I.IsLeftTriggerUp()) {
                                m_vrKeysToRemove.Add(vrKey);
                                NotifyKeyboardEvents(Key.LEFT_TRIGGER, KeyAction.Release);
                            }
                            if (KeyMap.VRButtonLookup[vrKey] == Key.RIGHT_TRIGGER && VRControl.I.IsRightTriggerUp()) {
                                m_vrKeysToRemove.Add(vrKey);
                                NotifyKeyboardEvents(Key.RIGHT_TRIGGER, KeyAction.Release);
                            }
                        }
                    }

                    foreach (string vKey in m_vrKeysToRemove) {
                        m_heldKeyCodes.Remove(vKey);
                    }
                    m_vrKeysToRemove.Clear();
                }
            }
        }

        private void NotifyKeyboardEvents(Key theKey, KeyAction keyAction){
            // Notify keypress listeners
            for(int i = 0; i < m_KeyPressListeners.Count; i++){
                m_KeyPressListeners[i].NotifyEvent(theKey, keyAction);
            }
            if (m_objectMover.HasObjects() && keyAction != KeyAction.Release) {
                m_objectMover.NotifyEvent(theKey);
            }
        }

        private void StartKeyRepeater() {
            if (!m_repeat_key_routine) {
                m_repeat_key_routine = Routine.Start(FireMultipleRoutine());
            }
        }

        private IEnumerator FireMultipleRoutine() {
            while (true) {
                foreach (KeyCode vKey in m_heldKeys) {
                    if (KeyMap.TweedleKeyLookup.ContainsKey(vKey))
                        NotifyKeyboardEvents(KeyMap.TweedleKeyLookup[vKey], KeyAction.Repeat);
                }
                yield return 0.02f;
            }
        }

    }
}
