using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;

public static class KeyMap
{
    public static Dictionary<KeyCode, Key> TweedleKeyLookup = new Dictionary<KeyCode, Key>()
    {
        {KeyCode.Alpha0, Key.DIGIT_0},
        {KeyCode.Keypad0, Key.NUMPAD0},
        {KeyCode.Alpha1, Key.DIGIT_1},
        {KeyCode.Keypad1, Key.NUMPAD1},
        {KeyCode.Alpha2, Key.DIGIT_2},
        {KeyCode.Keypad2, Key.NUMPAD2},
        {KeyCode.Alpha3, Key.DIGIT_3},
        {KeyCode.Keypad3, Key.NUMPAD3},
        {KeyCode.Alpha4, Key.DIGIT_4},
        {KeyCode.Keypad4, Key.NUMPAD4},
        {KeyCode.Alpha5, Key.DIGIT_5},
        {KeyCode.Keypad5, Key.NUMPAD5},
        {KeyCode.Alpha6, Key.DIGIT_6},
        {KeyCode.Keypad6, Key.NUMPAD6},
        {KeyCode.Alpha7, Key.DIGIT_7},
        {KeyCode.Keypad7, Key.NUMPAD7},
        {KeyCode.Alpha8, Key.DIGIT_8},
        {KeyCode.Keypad8, Key.NUMPAD8},
        {KeyCode.Alpha9, Key.DIGIT_9},
        {KeyCode.Keypad9, Key.NUMPAD9},
        {KeyCode.A, Key.A},
        {KeyCode.B, Key.B},
        {KeyCode.C, Key.C},
        {KeyCode.D, Key.D},
        {KeyCode.E, Key.E},
        {KeyCode.F, Key.F},
        {KeyCode.G, Key.G},
        {KeyCode.H, Key.H},
        {KeyCode.I, Key.I},
        {KeyCode.J, Key.J},
        {KeyCode.K, Key.K},
        {KeyCode.L, Key.L},
        {KeyCode.M, Key.M},
        {KeyCode.N, Key.N},
        {KeyCode.O, Key.O},
        {KeyCode.P, Key.P},
        {KeyCode.Q, Key.Q},
        {KeyCode.R, Key.R},
        {KeyCode.S, Key.S},
        {KeyCode.T, Key.T},
        {KeyCode.U, Key.U},
        {KeyCode.V, Key.V},
        {KeyCode.W, Key.W},
        {KeyCode.X, Key.X},
        {KeyCode.Y, Key.Y},
        {KeyCode.Z, Key.Z},
        {KeyCode.Space, Key.SPACE},
        {KeyCode.Comma, Key.COMMA},
        {KeyCode.Tab, Key.TAB},
        {KeyCode.CapsLock, Key.CAPS_LOCK},
        {KeyCode.BackQuote, Key.BACK_QUOTE},
        {KeyCode.Slash, Key.SLASH},
        {KeyCode.Backslash, Key.BACK_SLASH},
        {KeyCode.Period, Key.PERIOD},
        {KeyCode.Semicolon, Key.SEMICOLON},
        {KeyCode.Quote, Key.QUOTE},
        {KeyCode.LeftBracket, Key.OPEN_BRACKET},
        {KeyCode.RightBracket, Key.CLOSE_BRACKET},
        {KeyCode.Backspace, Key.BACK_SPACE},
        {KeyCode.Insert, Key.INSERT},
        {KeyCode.Home, Key.HOME},
        {KeyCode.PageUp, Key.PAGE_UP},
        {KeyCode.Delete, Key.DELETE},
        {KeyCode.End, Key.END},
        {KeyCode.PageDown, Key.PAGE_DOWN},
        {KeyCode.Numlock, Key.NUM_LOCK},
        {KeyCode.Asterisk, Key.ASTERISK},
        {KeyCode.KeypadMinus, Key.MINUS},
        {KeyCode.KeypadPlus, Key.PLUS},
        {KeyCode.KeypadPeriod, Key.PERIOD},
        {KeyCode.F1, Key.F1},
        {KeyCode.F2, Key.F2},
        {KeyCode.F3, Key.F3},
        {KeyCode.F4, Key.F4},
        {KeyCode.F5, Key.F5},
        {KeyCode.F6, Key.F6},
        {KeyCode.F7, Key.F7},
        {KeyCode.F8, Key.F8},
        {KeyCode.F9, Key.F9},
        {KeyCode.F10, Key.F10},
        {KeyCode.F11, Key.F11},
        {KeyCode.F12, Key.F12},
        {KeyCode.LeftShift, Key.SHIFT},
        {KeyCode.RightShift, Key.SHIFT},
        {KeyCode.LeftAlt, Key.ALT},
        {KeyCode.RightAlt, Key.ALT},
        {KeyCode.LeftControl, Key.CONTROL},
        {KeyCode.RightControl, Key.CONTROL},
        {KeyCode.KeypadEnter, Key.ENTER},
        {KeyCode.Return, Key.ENTER},
        {KeyCode.LeftArrow, Key.LEFT},
        {KeyCode.RightArrow, Key.RIGHT},
        {KeyCode.UpArrow, Key.UP},
        {KeyCode.DownArrow, Key.DOWN}
    };

    public static List<string> VRButtonStrings = new List<string>() {
        "RightTrigger",
        "RightGrip",
        "PrimaryRight",
        "SecondaryRight",
        "MenuRight",
        "LeftTrigger",
        "LeftGrip",
        "PrimaryLeft",
        "SecondaryLeft",
        "MenuLeft",
        "RightThumbstickClick",
        "LeftThumbstickClick"
    };

    public static List<string> VRAxisStrings = new List<string>(){
        "RightThumbstickUpDown",
        "RightThumbstickLeftRight",
        "LeftThumbstickUpDown",
        "LeftThumbstickLeftRight"
    };

    public static Dictionary<string, Key> VRButtonLookup = new Dictionary<string, Key>()
    {
        {"RightTrigger", Key.RIGHT_TRIGGER},
        {"RightGrip", Key.RIGHT_GRIP},
        {"PrimaryRight", Key.RIGHT_PRIMARY},
        {"SecondaryRight", Key.RIGHT_SECONDARY},
        {"MenuRight", Key.RIGHT_MENU},
        {"LeftTrigger", Key.LEFT_TRIGGER},
        {"LeftGrip", Key.LEFT_GRIP},
        {"PrimaryLeft", Key.LEFT_PRIMARY},
        {"SecondaryLeft", Key.LEFT_SECONDARY},
        {"MenuLeft", Key.LEFT_MENU},
        {"RightThumbstickClick", Key.RIGHT_THUMBSTICK_CLICK},
        {"LeftThumbstickClick", Key.LEFT_THUMBSTICK_CLICK}
    };

    public static Dictionary<string, Key> VRAxisLookup = new Dictionary<string, Key>()
    {
        {"RightThumbstickUpDown_P", Key.RIGHT_AXIS_UP},
        {"RightThumbstickUpDown_N", Key.RIGHT_AXIS_DOWN},
        {"RightThumbstickLeftRight_N", Key.RIGHT_AXIS_LEFT},
        {"RightThumbstickLeftRight_P", Key.RIGHT_AXIS_RIGHT},
        {"LeftThumbstickUpDown_P", Key.LEFT_AXIS_UP},
        {"LeftThumbstickUpDown_N", Key.LEFT_AXIS_DOWN},
        {"LeftThumbstickLeftRight_N", Key.LEFT_AXIS_LEFT},
        {"LeftThumbstickLeftRight_P", Key.LEFT_AXIS_RIGHT}
    };
}

        
