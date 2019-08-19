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
        private ObjectKeyboardMover m_objectMover = new ObjectKeyboardMover();

        public void AddListener(KeyEventListnerProxy listener)
        {
            m_KeyPressListeners.Add(listener);
        }

        public void AddObjectMover(Transform objectToMove)
        {
            m_objectMover.AddMover(objectToMove);
        }

        public void RemoveAllKeys()
        {
            m_heldKeys.Clear();
            m_objectMover.ClearHeldKeys();
        }

        // Called from Update()
        public void HandleKeyboardEvents()
        {
            if(m_KeyPressListeners.Count > 0 || m_objectMover.GetNumMovers() > 0)
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
                            //break;
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
                            //break;
                        }
                    }
                }
            }
        }

        private void NotifyKeyboardEvents(int theKey, bool keyDown)
        {
            // Notify keypress listeners
            for(int i = 0; i < m_KeyPressListeners.Count; i++){
                m_KeyPressListeners[i].NotifyEvent(theKey, keyDown);
            }
            // Notify object movers
            for (int i = 0; i < m_objectMover.GetNumMovers(); i++){
                m_objectMover.NotifyEvent(theKey, keyDown);
            }

        }

        private void CheckNotifyKey(KeyCode key, bool checkDown)
        {
            switch(key)
            {
                // 0
                case KeyCode.Alpha0:  NotifyKeyboardEvents((int)Key.DIGIT_0, checkDown); break;
                case KeyCode.Keypad0: NotifyKeyboardEvents((int)Key.NUMPAD0, checkDown); break;
                // 1
                case KeyCode.Alpha1: NotifyKeyboardEvents((int)Key.DIGIT_1, checkDown); break;
                case KeyCode.Keypad1: NotifyKeyboardEvents((int)Key.NUMPAD1, checkDown); break;
                // 2
                case KeyCode.Alpha2: NotifyKeyboardEvents((int)Key.DIGIT_2, checkDown); break;
                case KeyCode.Keypad2: NotifyKeyboardEvents((int)Key.NUMPAD2, checkDown); break;
                // 3
                case KeyCode.Alpha3: NotifyKeyboardEvents((int)Key.DIGIT_3, checkDown); break;
                case KeyCode.Keypad3: NotifyKeyboardEvents((int)Key.NUMPAD3, checkDown); break;
                // 4
                case KeyCode.Alpha4: NotifyKeyboardEvents((int)Key.DIGIT_4, checkDown); break;
                case KeyCode.Keypad4: NotifyKeyboardEvents((int)Key.NUMPAD4, checkDown); break;
                // 5
                case KeyCode.Alpha5: NotifyKeyboardEvents((int)Key.DIGIT_5, checkDown); break;
                case KeyCode.Keypad5: NotifyKeyboardEvents((int)Key.NUMPAD5, checkDown); break;
                // 6
                case KeyCode.Alpha6: NotifyKeyboardEvents((int)Key.DIGIT_6, checkDown); break;
                case KeyCode.Keypad6: NotifyKeyboardEvents((int)Key.NUMPAD6, checkDown); break;
                // 7
                case KeyCode.Alpha7: NotifyKeyboardEvents((int)Key.DIGIT_7, checkDown); break;
                case KeyCode.Keypad7: NotifyKeyboardEvents((int)Key.NUMPAD7, checkDown); break;
                // 8
                case KeyCode.Alpha8: NotifyKeyboardEvents((int)Key.DIGIT_8, checkDown); break;
                case KeyCode.Keypad8: NotifyKeyboardEvents((int)Key.NUMPAD8, checkDown); break;
                // 9
                case KeyCode.Alpha9: NotifyKeyboardEvents((int)Key.DIGIT_9, checkDown); break;
                case KeyCode.Keypad9: NotifyKeyboardEvents((int)Key.NUMPAD9, checkDown); break;

                case KeyCode.A: NotifyKeyboardEvents((int)Key.A, checkDown); break;
                case KeyCode.B: NotifyKeyboardEvents((int)Key.B, checkDown); break;
                case KeyCode.C: NotifyKeyboardEvents((int)Key.C, checkDown); break;
                case KeyCode.D: NotifyKeyboardEvents((int)Key.D, checkDown); break;
                case KeyCode.E: NotifyKeyboardEvents((int)Key.E, checkDown); break;
                case KeyCode.F: NotifyKeyboardEvents((int)Key.F, checkDown); break;
                case KeyCode.G: NotifyKeyboardEvents((int)Key.G, checkDown); break;
                case KeyCode.H: NotifyKeyboardEvents((int)Key.H, checkDown); break;
                case KeyCode.I: NotifyKeyboardEvents((int)Key.I, checkDown); break;
                case KeyCode.J: NotifyKeyboardEvents((int)Key.J, checkDown); break;
                case KeyCode.K: NotifyKeyboardEvents((int)Key.K, checkDown); break;
                case KeyCode.L: NotifyKeyboardEvents((int)Key.L, checkDown); break;
                case KeyCode.M: NotifyKeyboardEvents((int)Key.M, checkDown); break;
                case KeyCode.N: NotifyKeyboardEvents((int)Key.N, checkDown); break;
                case KeyCode.O: NotifyKeyboardEvents((int)Key.O, checkDown); break;
                case KeyCode.P: NotifyKeyboardEvents((int)Key.P, checkDown); break;
                case KeyCode.Q: NotifyKeyboardEvents((int)Key.Q, checkDown); break;
                case KeyCode.R: NotifyKeyboardEvents((int)Key.R, checkDown); break;
                case KeyCode.S: NotifyKeyboardEvents((int)Key.S, checkDown); break;
                case KeyCode.T: NotifyKeyboardEvents((int)Key.T, checkDown); break;
                case KeyCode.U: NotifyKeyboardEvents((int)Key.U, checkDown); break;
                case KeyCode.V: NotifyKeyboardEvents((int)Key.V, checkDown); break;
                case KeyCode.W: NotifyKeyboardEvents((int)Key.W, checkDown); break;
                case KeyCode.X: NotifyKeyboardEvents((int)Key.X, checkDown); break;
                case KeyCode.Y: NotifyKeyboardEvents((int)Key.Y, checkDown); break;
                case KeyCode.Z: NotifyKeyboardEvents((int)Key.Z, checkDown); break;

                case KeyCode.Space: NotifyKeyboardEvents((int)Key.SPACE, checkDown); break;
                case KeyCode.Comma: NotifyKeyboardEvents((int)Key.COMMA, checkDown); break;
                case KeyCode.Tab: NotifyKeyboardEvents((int)Key.TAB, checkDown); break;
                case KeyCode.CapsLock: NotifyKeyboardEvents((int)Key.CAPS_LOCK, checkDown); break;
                case KeyCode.BackQuote: NotifyKeyboardEvents((int)Key.BACK_QUOTE, checkDown); break;
                case KeyCode.Slash: NotifyKeyboardEvents((int)Key.SLASH, checkDown); break;
                case KeyCode.Backslash: NotifyKeyboardEvents((int)Key.BACK_SLASH, checkDown); break;
                case KeyCode.Period: NotifyKeyboardEvents((int)Key.PERIOD, checkDown); break;
                case KeyCode.Semicolon: NotifyKeyboardEvents((int)Key.SEMICOLON, checkDown); break;
                case KeyCode.Quote: NotifyKeyboardEvents((int)Key.QUOTE, checkDown); break;
                case KeyCode.LeftBracket: NotifyKeyboardEvents((int)Key.OPEN_BRACKET, checkDown); break;
                case KeyCode.RightBracket: NotifyKeyboardEvents((int)Key.CLOSE_BRACKET, checkDown); break;
                case KeyCode.Backspace: NotifyKeyboardEvents((int)Key.BACK_SPACE, checkDown); break;
                case KeyCode.Insert: NotifyKeyboardEvents((int)Key.INSERT, checkDown); break;
                case KeyCode.Home: NotifyKeyboardEvents((int)Key.HOME, checkDown); break;
                case KeyCode.PageUp: NotifyKeyboardEvents((int)Key.PAGE_UP, checkDown); break;
                case KeyCode.Delete: NotifyKeyboardEvents((int)Key.DELETE, checkDown); break;
                case KeyCode.End: NotifyKeyboardEvents((int)Key.END, checkDown); break;
                case KeyCode.PageDown: NotifyKeyboardEvents((int)Key.PAGE_DOWN, checkDown); break;
                case KeyCode.Numlock: NotifyKeyboardEvents((int)Key.NUM_LOCK, checkDown); break;
                case KeyCode.Asterisk: NotifyKeyboardEvents((int)Key.ASTERISK, checkDown); break;
                case KeyCode.KeypadMinus: NotifyKeyboardEvents((int)Key.MINUS, checkDown); break;
                case KeyCode.KeypadPlus: NotifyKeyboardEvents((int)Key.PLUS, checkDown); break;
                case KeyCode.KeypadPeriod: NotifyKeyboardEvents((int)Key.PERIOD, checkDown); break;
                case KeyCode.F1: NotifyKeyboardEvents((int)Key.F1, checkDown); break;
                case KeyCode.F2: NotifyKeyboardEvents((int)Key.F2, checkDown); break;
                case KeyCode.F3: NotifyKeyboardEvents((int)Key.F3, checkDown); break;
                case KeyCode.F4: NotifyKeyboardEvents((int)Key.F4, checkDown); break;
                case KeyCode.F5: NotifyKeyboardEvents((int)Key.F5, checkDown); break;
                case KeyCode.F6: NotifyKeyboardEvents((int)Key.F6, checkDown); break;
                case KeyCode.F7: NotifyKeyboardEvents((int)Key.F7, checkDown); break;
                case KeyCode.F8: NotifyKeyboardEvents((int)Key.F8, checkDown); break;
                case KeyCode.F9: NotifyKeyboardEvents((int)Key.F9, checkDown); break;
                case KeyCode.F10: NotifyKeyboardEvents((int)Key.F10, checkDown); break;
                case KeyCode.F11: NotifyKeyboardEvents((int)Key.F11, checkDown); break;
                case KeyCode.F12: NotifyKeyboardEvents((int)Key.F12, checkDown); break;

                case KeyCode.LeftShift:
                case KeyCode.RightShift: NotifyKeyboardEvents((int)Key.SHIFT, checkDown); break;
                case KeyCode.LeftAlt:
                case KeyCode.RightAlt: NotifyKeyboardEvents((int)Key.ALT, checkDown); break;
                case KeyCode.LeftControl:
                case KeyCode.RightControl: NotifyKeyboardEvents((int)Key.CONTROL, checkDown); break;
                case KeyCode.KeypadEnter:
                case KeyCode.Return: NotifyKeyboardEvents((int)Key.ENTER, checkDown); break;

                case KeyCode.LeftArrow: NotifyKeyboardEvents((int)Key.LEFT, checkDown); break;
                case KeyCode.RightArrow: NotifyKeyboardEvents((int)Key.RIGHT, checkDown); break;
                case KeyCode.UpArrow: NotifyKeyboardEvents((int)Key.UP, checkDown); break;
                case KeyCode.DownArrow: NotifyKeyboardEvents((int)Key.DOWN, checkDown); break;

            }
        }
    }
}
