using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;

namespace Alice.Player.Unity 
{
    public class KeyboardEventHandler
    {
        private List<KeyEventListnerProxy> m_KeyPressListeners = new List<KeyEventListnerProxy>();
        private List<KeyCode> m_heldKeys = new List<KeyCode>();

        public void AddListener(KeyEventListnerProxy listener)
        {
            m_KeyPressListeners.Add(listener);
        }

        public void HandleKeyboardEvents()
        {
            if(m_KeyPressListeners.Count > 0)
            {
                // Check for key down
                if(Input.anyKeyDown)
                {
                    foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                    {
                        if(Input.GetKeyDown(vKey))
                        {
                            m_heldKeys.Add(vKey);
                            CheckNotifyKey(vKey, true);
                            break;
                        }
                    }
                }

                // Check for key up if we've pressed one down in the past
                if(m_heldKeys.Count > 0)
                {
                    foreach(KeyCode vKey in m_heldKeys)
                    {
                        if(Input.GetKeyUp(vKey))
                        {
                            CheckNotifyKey(vKey, false);
                            break;
                        }
                    }
                }
            }
        }

        private void NotifyKeyboardEvents(bool isDigit, bool isLetter, int theKey, bool keyDown)
        {
            for(int i = 0; i < m_KeyPressListeners.Count; i++)
            {
                m_KeyPressListeners[i].NotifyEvent(isDigit, isLetter, theKey, keyDown);
            }
        }

        private void CheckNotifyKey(KeyCode key, bool checkDown)
        {
            switch(key)
            {
                // 0
                case KeyCode.Alpha0:  NotifyKeyboardEvents(true, false, (int)Key.DIGIT_0, checkDown); break;
                case KeyCode.Keypad0: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD0, checkDown); break;

                // 1
                case KeyCode.Alpha1: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad1: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD1, checkDown); break;

                // 2
                case KeyCode.Alpha2: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad2: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD2, checkDown); break;

                // 3
                case KeyCode.Alpha3: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad3: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD3, checkDown); break;

                // 4
                case KeyCode.Alpha4: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad4: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD4, checkDown); break;

                // 5
                case KeyCode.Alpha5: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad5: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD5, checkDown); break;

                // 6
                case KeyCode.Alpha6: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad6: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD6, checkDown); break;

                // 7
                case KeyCode.Alpha7: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad7: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD7, checkDown); break;

                // 8
                case KeyCode.Alpha8: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad8: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD8, checkDown); break;

                // 9
                case KeyCode.Alpha9: NotifyKeyboardEvents(true, false, (int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad9: NotifyKeyboardEvents(true, false, (int)Key.NUMPAD9, checkDown); break;

                // A
                case KeyCode.A: NotifyKeyboardEvents(false, true, (int)Key.A, checkDown); break;

                // B
                case KeyCode.B: NotifyKeyboardEvents(false, true, (int)Key.B, checkDown); break;

                // C
                case KeyCode.C: NotifyKeyboardEvents(false, true, (int)Key.C, checkDown); break;

                // D
                case KeyCode.D: NotifyKeyboardEvents(false, true, (int)Key.D, checkDown); break;

                // E
                case KeyCode.E: NotifyKeyboardEvents(false, true, (int)Key.E, checkDown); break;

                // F
                case KeyCode.F: NotifyKeyboardEvents(false, true, (int)Key.F, checkDown); break;

                // G
                case KeyCode.G: NotifyKeyboardEvents(false, true, (int)Key.G, checkDown); break;

                // H
                case KeyCode.H: NotifyKeyboardEvents(false, true, (int)Key.H, checkDown); break;

                // I
                case KeyCode.I: NotifyKeyboardEvents(false, true, (int)Key.I, checkDown); break;

                // J
                case KeyCode.J: NotifyKeyboardEvents(false, true, (int)Key.J, checkDown); break;

                // K
                case KeyCode.K: NotifyKeyboardEvents(false, true, (int)Key.K, checkDown); break;

                // L
                case KeyCode.L: NotifyKeyboardEvents(false, true, (int)Key.L, checkDown); break;

                // M
                case KeyCode.M: NotifyKeyboardEvents(false, true, (int)Key.M, checkDown); break;

                // N
                case KeyCode.N: NotifyKeyboardEvents(false, true, (int)Key.N, checkDown); break;

                // O
                case KeyCode.O: NotifyKeyboardEvents(false, true, (int)Key.O, checkDown); break;

                // P
                case KeyCode.P: NotifyKeyboardEvents(false, true, (int)Key.P, checkDown); break;

                // Q
                case KeyCode.Q: NotifyKeyboardEvents(false, true, (int)Key.Q, checkDown); break;

                // R
                case KeyCode.R: NotifyKeyboardEvents(false, true, (int)Key.R, checkDown); break;

                // S
                case KeyCode.S: NotifyKeyboardEvents(false, true, (int)Key.S, checkDown); break;

                // T
                case KeyCode.T: NotifyKeyboardEvents(false, true, (int)Key.T, checkDown); break;

                // U
                case KeyCode.U: NotifyKeyboardEvents(false, true, (int)Key.U, checkDown); break;

                // V
                case KeyCode.V: NotifyKeyboardEvents(false, true, (int)Key.V, checkDown); break;

                // W
                case KeyCode.W: NotifyKeyboardEvents(false, true, (int)Key.W, checkDown); break;

                // X
                case KeyCode.X: NotifyKeyboardEvents(false, true, (int)Key.X, checkDown); break;

                // Y
                case KeyCode.Y: NotifyKeyboardEvents(false, true, (int)Key.Y, checkDown); break;

                // Z
                case KeyCode.Z: NotifyKeyboardEvents(false, true, (int)Key.Z, checkDown); break;

                case KeyCode.Space: NotifyKeyboardEvents(false, false, (int)Key.SPACE, checkDown); break;

                case KeyCode.LeftShift:
                case KeyCode.RightShift: NotifyKeyboardEvents(false, false, (int)Key.SHIFT, checkDown); break;

                case KeyCode.LeftAlt:
                case KeyCode.RightAlt: NotifyKeyboardEvents(false, false, (int)Key.ALT, checkDown); break;

                case KeyCode.LeftControl:
                case KeyCode.RightControl: NotifyKeyboardEvents(false, false, (int)Key.CONTROL, checkDown); break;

                case KeyCode.KeypadEnter:
                case KeyCode.Return: NotifyKeyboardEvents(false, false, (int)Key.ENTER, checkDown); break;

                // Arrow keys
                case KeyCode.LeftArrow: NotifyKeyboardEvents(false, false, (int)Key.LEFT, checkDown); break;
                case KeyCode.RightArrow: NotifyKeyboardEvents(false, false, (int)Key.RIGHT, checkDown); break;
                case KeyCode.UpArrow: NotifyKeyboardEvents(false, false, (int)Key.UP, checkDown); break;
                case KeyCode.DownArrow: NotifyKeyboardEvents(false, false, (int)Key.DOWN, checkDown); break;

            }
        }
    }
}
