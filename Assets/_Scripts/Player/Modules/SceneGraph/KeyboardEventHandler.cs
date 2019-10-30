using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using UnityEngine.XR;
using BeauRoutine;
using System;

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
        private ObjectKeyboardMover m_objectMover = new ObjectKeyboardMover();
        private HashSet<KeyCode> m_heldKeyCodes = new HashSet<KeyCode>();
        private HashSet<Key> m_heldKeys = new HashSet<Key>();
        private HashSet<Key> m_releasedKeys = new HashSet<Key>();

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
            m_heldKeyCodes.Clear();
            m_heldKeys.Clear();
            m_releasedKeys.Clear();
        }

        public void HandleKeyboardEvents(){
            if (m_KeyPressListeners.Count <= 0 && !m_objectMover.HasObjects())
                return;
            AddPressedKeys();
            if (XRSettings.enabled)
                UpdateControllerEvents();
            RemoveReleasedKeys();
        }

        private void AddPressedKeys(){
            if (Input.anyKeyDown){
                foreach (KeyCode vKey in Enum.GetValues(typeof(KeyCode))){
                    if (Input.GetKeyDown(vKey) && KeyMap.TweedleKeyLookup.ContainsKey(vKey) && m_heldKeyCodes.Add(vKey)){
                        Press(KeyMap.TweedleKeyLookup[vKey]);
                        //Would be nice to not need to check every key here. But we want to support more than one key press at a time, so no break;
                    }
                }
            }
        }

        private void RemoveReleasedKeys(){
            if (m_heldKeyCodes.Count > 0){
                HashSet<KeyCode> releasedKeyCodes = new HashSet<KeyCode>();
                foreach (KeyCode vKey in m_heldKeyCodes){
                    if (Input.GetKeyUp(vKey)){
                        if (KeyMap.TweedleKeyLookup.ContainsKey(vKey))
                            releasedKeyCodes.Add(vKey);
                        Release(KeyMap.TweedleKeyLookup[vKey]);
                    }
                }
                foreach (KeyCode vKey in releasedKeyCodes){
                    m_heldKeyCodes.Remove(vKey);
                }
            }
        }

        private void UpdateControllerEvents(){
            if (Input.anyKeyDown){
                foreach (string buttonString in KeyMap.VRButtonLookup.Keys){
                    if (Input.GetButtonDown(buttonString)){
                        Press(KeyMap.VRButtonLookup[buttonString]);
                    }
                }
            }
            foreach (string buttonString in KeyMap.VRButtonLookup.Keys){
                if (Input.GetButtonUp(buttonString)){
                    Release(KeyMap.VRButtonLookup[buttonString]);
                }
            }
            foreach (string axisString in KeyMap.AxisKeyPairs.Keys){
                float axisValue = Input.GetAxis(axisString);
                Tuple<Key, Key> keys;
                if (KeyMap.AxisKeyPairs.TryGetValue(axisString, out keys)){
                    if (axisValue > VRControl.TRIGGER_SENSITIVITY){
                        Press(keys.Item2);
                        Release(keys.Item1);
                    } else if (axisValue < -VRControl.TRIGGER_SENSITIVITY){
                        Press(keys.Item1);
                        Release(keys.Item2);
                    } else{
                        Release(keys.Item1);
                        Release(keys.Item2);
                    }
                }
            }

            if (VRControl.IsLeftTriggerDown())
                Press(Key.LEFT_TRIGGER);
            if (VRControl.IsRightTriggerDown())
                Press(Key.RIGHT_TRIGGER);
            if (VRControl.IsLeftTriggerUp())
                Release(Key.LEFT_TRIGGER);
            if (VRControl.IsRightTriggerUp())
                Release(Key.RIGHT_TRIGGER);
        }

        private void Press(Key key){
            if (m_heldKeys.Add(key))
                NotifyListeners(key, KeyAction.Press);
        }

        private void Release(Key key){
            if (m_heldKeys.Contains(key) && m_releasedKeys.Add(key))
                NotifyListeners(key, KeyAction.Release);
        }

        private void StartKeyRepeater(){
            if (!m_repeat_key_routine)
                m_repeat_key_routine = Routine.Start(FireMultipleRoutine());
        }

        private IEnumerator FireMultipleRoutine(){
            while (true){
                foreach (Key key in m_heldKeys){
                    if (!m_releasedKeys.Contains(key))
                        NotifyListeners(key, KeyAction.Repeat);
                }
                foreach (Key key in m_releasedKeys){
                    m_heldKeys.Remove(key);
                }
                m_releasedKeys.Clear();
                yield return null;
            }
        }

        private void NotifyListeners(Key theKey, KeyAction keyAction){
            for (int i = 0; i < m_KeyPressListeners.Count; i++){
                m_KeyPressListeners[i].NotifyEvent(theKey, keyAction);
            }
            if (m_objectMover.HasObjects() && keyAction != KeyAction.Release)
                m_objectMover.NotifyEvent(theKey);
        }
    }
}
