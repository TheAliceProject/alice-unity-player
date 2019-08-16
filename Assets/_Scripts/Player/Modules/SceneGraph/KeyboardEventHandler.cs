using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;

namespace Alice.Player.Unity 
{
    public class KeyboardEventHandler
    {
        private List<KeyEventListenerProxy> m_KeyPressListeners = new List<KeyEventListenerProxy>();
        private List<KeyCode> m_heldKeys = new List<KeyCode>();
        private ObjectKeyboardMover m_objectMover = new ObjectKeyboardMover();
        private List<KeyCode> m_keysToRemove = new List<KeyCode>();

        public void AddListener(KeyEventListenerProxy listener){
            m_KeyPressListeners.Add(listener);
        }

        public void AddObjectMover(Transform objectToMove){
            m_objectMover.AddMover(objectToMove);
        }

        public void RemoveAllKeys(){
            m_heldKeys.Clear();
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
