﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;
using BeauRoutine;

namespace Alice.Player.Unity {
    public class ObjectKeyboardMover{

        private Routine m_routine;
        private List<Transform> m_ObjectMovers = new List<Transform>();
    
        private List<Key> m_arrowKeys = new List<Key> { Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, 
                                                        Key.W, Key.A, Key.S, Key.D,
                                                        Key.RIGHT_AXIS_UP, Key.RIGHT_AXIS_DOWN, Key.RIGHT_AXIS_LEFT, Key.RIGHT_AXIS_RIGHT,
                                                        Key.LEFT_AXIS_UP, Key.LEFT_AXIS_DOWN, Key.LEFT_AXIS_LEFT, Key.LEFT_AXIS_RIGHT};
        private List<Key> m_currentlyHeldKeys = new List<Key>();

        private const float UP_DOWN_SCALE_FACTOR = 0.055f;
        private const float LEFT_RIGHT_SCALE_FACTOR = 1.612f;

        public void AddMover(Transform objectToMove){
            m_ObjectMovers.Add(objectToMove);
            if (!m_routine){
                m_routine = Routine.Start(FireMultipleRoutine());
            }
        }

        public int GetNumMovers(){
            return m_ObjectMovers.Count;
        }

        public void ClearHeldKeys(){
            m_currentlyHeldKeys.Clear();
        }

        public void NotifyEvent(int theKey, bool keyDown){
            if(!m_arrowKeys.Contains((Key)theKey))
                return;

            // Manage key downs and key ups in regards to the held key policy
            if(keyDown && !m_currentlyHeldKeys.Contains((Key)theKey)){
                m_currentlyHeldKeys.Add((Key)theKey);
            }
            else{ // key up
                m_currentlyHeldKeys.Remove((Key)theKey);
            }
        }

        private IEnumerator FireMultipleRoutine(){
            while(true){
                for (int i = 0; i < m_ObjectMovers.Count; i++){
                    for (int j = 0; j < m_currentlyHeldKeys.Count; j++){
                        Key currentlyHeld = m_currentlyHeldKeys[j];
                        if (currentlyHeld == Key.W || currentlyHeld == Key.UP || currentlyHeld == Key.LEFT_AXIS_UP || currentlyHeld == Key.RIGHT_AXIS_UP){
                            Debug.Log("Moving " + m_ObjectMovers.Count + " " + m_currentlyHeldKeys.Count);
                            m_ObjectMovers[i].position -= (m_ObjectMovers[i].forward * UP_DOWN_SCALE_FACTOR);
                        }
                        if (currentlyHeld == Key.S || currentlyHeld == Key.DOWN || currentlyHeld == Key.LEFT_AXIS_DOWN || currentlyHeld == Key.RIGHT_AXIS_DOWN){
                            m_ObjectMovers[i].position += (m_ObjectMovers[i].forward * UP_DOWN_SCALE_FACTOR);
                        }
                        if (currentlyHeld == Key.A || currentlyHeld == Key.LEFT || currentlyHeld == Key.LEFT_AXIS_LEFT || currentlyHeld == Key.RIGHT_AXIS_LEFT){
                            m_ObjectMovers[i].SetRotation(m_ObjectMovers[i].localRotation.eulerAngles.y - LEFT_RIGHT_SCALE_FACTOR, Axis.Y, Space.Self);
                        }
                        if (currentlyHeld == Key.D || currentlyHeld == Key.RIGHT || currentlyHeld == Key.LEFT_AXIS_RIGHT || currentlyHeld == Key.RIGHT_AXIS_RIGHT){
                            m_ObjectMovers[i].SetRotation(m_ObjectMovers[i].localRotation.eulerAngles.y + LEFT_RIGHT_SCALE_FACTOR, Axis.Y, Space.Self);
                        }
                    }
                }
                yield return 0.02f;
            }
        }
    }
}
