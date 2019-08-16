using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    public enum Key {
        ENTER = 0,
        BACK_SPACE,
        TAB,
        CANCEL,
        CLEAR,
        SHIFT,
        CONTROL,
        ALT,
        PAUSE,
        CAPS_LOCK,
        ESCAPE,
        SPACE,
        PAGE_UP,
        PAGE_DOWN,
        END,
        HOME,
        LEFT,
        UP,
        RIGHT,
        DOWN,
        COMMA,
        MINUS,
        PERIOD,
        SLASH,
        DIGIT_0,
        DIGIT_1,
        DIGIT_2,
        DIGIT_3,
        DIGIT_4,
        DIGIT_5,
        DIGIT_6,
        DIGIT_7,
        DIGIT_8,
        DIGIT_9,
        SEMICOLON,
        EQUALS,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        OPEN_BRACKET,
        BACK_SLASH,
        CLOSE_BRACKET,
        NUMPAD0,
        NUMPAD1,
        NUMPAD2,
        NUMPAD3,
        NUMPAD4,
        NUMPAD5,
        NUMPAD6,
        NUMPAD7,
        NUMPAD8,
        NUMPAD9,
        MULTIPLY,
        ADD,
        SEPARATOR,
        SUBTRACT,
        DECIMAL,
        DIVIDE,
        DELETE,
        NUM_LOCK,
        SCROLL_LOCK,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        PRINTSCREEN,
        INSERT,
        HELP,
        META,
        BACK_QUOTE,
        QUOTE,
        KP_UP,
        KP_DOWN,
        KP_LEFT,
        KP_RIGHT,
        DEAD_GRAVE,
        DEAD_ACUTE,
        DEAD_CIRCUMFLEX,
        DEAD_TILDE,
        DEAD_MACRON,
        DEAD_BREVE,
        DEAD_ABOVEDOT,
        DEAD_DIAERESIS,
        DEAD_ABOVERING,
        DEAD_DOUBLEACUTE,
        DEAD_CARON,
        DEAD_CEDILLA,
        DEAD_OGONEK,
        DEAD_IOTA,
        DEAD_VOICED_SOUND,
        DEAD_SEMIVOICED_SOUND,
        AMPERSAND,
        ASTERISK,
        QUOTEDBL,
        LESS,
        GREATER,
        BRACELEFT,
        BRACERIGHT,
        AT,
        COLON,
        CIRCUMFLEX,
        DOLLAR,
        EURO_SIGN,
        EXCLAMATION_MARK,
        INVERTED_EXCLAMATION_MARK,
        LEFT_PARENTHESIS,
        NUMBER_SIGN,
        PLUS,
        RIGHT_PARENTHESIS,
        UNDERSCORE,
        WINDOWS,
        CONTEXT_MENU,
        FINAL,
        CONVERT,
        NONCONVERT,
        ACCEPT,
        MODECHANGE,
        KANA,
        KANJI,
        ALPHANUMERIC,
        KATAKANA,
        HIRAGANA,
        FULL_WIDTH,
        HALF_WIDTH,
        ROMAN_CHARACTERS,
        ALL_CANDIDATES,
        PREVIOUS_CANDIDATE,
        CODE_INPUT,
        JAPANESE_KATAKANA,
        JAPANESE_HIRAGANA,
        JAPANESE_ROMAN,
        KANA_LOCK,
        INPUT_METHOD_ON_OFF,
        CUT,
        COPY,
        PASTE,
        UNDO,
        AGAIN,
        FIND,
        PROPS,
        STOP,
        COMPOSE,
        ALT_GRAPH,
        BEGIN,
        UNDEFINED
    }

    [PInteropType("Key")]
    static public class KeyModule
    {
        [PInteropField]
        public const int ENTER = (int)Key.ENTER;
        [PInteropField]
        public const int BACK_SPACE = (int)Key.BACK_SPACE;
        [PInteropField]
        public const int TAB = (int)Key.TAB;
        [PInteropField]
        public const int CANCEL = (int)Key.CANCEL;
        [PInteropField]
        public const int CLEAR = (int)Key.CLEAR;
        [PInteropField]
        public const int SHIFT = (int)Key.SHIFT;
        [PInteropField]
        public const int CONTROL = (int)Key.CONTROL;
        [PInteropField]
        public const int ALT = (int)Key.ALT;
        [PInteropField]
        public const int PAUSE = (int)Key.PAUSE;
        [PInteropField]
        public const int CAPS_LOCK = (int)Key.CAPS_LOCK;
        [PInteropField]
        public const int ESCAPE = (int)Key.ESCAPE;
        [PInteropField]
        public const int SPACE = (int)Key.SPACE;
        [PInteropField]
        public const int PAGE_UP = (int)Key.PAGE_UP;
        [PInteropField]
        public const int PAGE_DOWN = (int)Key.PAGE_DOWN;
        [PInteropField]
        public const int END = (int)Key.END;
        [PInteropField]
        public const int HOME = (int)Key.HOME;
        [PInteropField]
        public const int LEFT = (int)Key.LEFT;
        [PInteropField]
        public const int UP = (int)Key.UP;
        [PInteropField]
        public const int RIGHT = (int)Key.RIGHT;
        [PInteropField]
        public const int DOWN = (int)Key.DOWN;
        [PInteropField]
        public const int COMMA = (int)Key.COMMA;
        [PInteropField]
        public const int MINUS = (int)Key.MINUS;
        [PInteropField]
        public const int PERIOD = (int)Key.PERIOD;
        [PInteropField]
        public const int SLASH = (int)Key.SLASH;
        [PInteropField]
        public const int DIGIT_0 = (int)Key.DIGIT_0;
        [PInteropField]
        public const int DIGIT_1 = (int)Key.DIGIT_1;
        [PInteropField]
        public const int DIGIT_2 = (int)Key.DIGIT_2;
        [PInteropField]
        public const int DIGIT_3 = (int)Key.DIGIT_3;
        [PInteropField]
        public const int DIGIT_4 = (int)Key.DIGIT_4;
        [PInteropField]
        public const int DIGIT_5 = (int)Key.DIGIT_5;
        [PInteropField]
        public const int DIGIT_6 = (int)Key.DIGIT_6;
        [PInteropField]
        public const int DIGIT_7 = (int)Key.DIGIT_7;
        [PInteropField]
        public const int DIGIT_8 = (int)Key.DIGIT_8;
        [PInteropField]
        public const int DIGIT_9 = (int)Key.DIGIT_9;
        [PInteropField]
        public const int SEMICOLON = (int)Key.SEMICOLON;
        [PInteropField]
        public const int EQUALS = (int)Key.EQUALS;
        [PInteropField]
        public const int A = (int)Key.A;
        [PInteropField]
        public const int B = (int)Key.B;
        [PInteropField]
        public const int C = (int)Key.C;
        [PInteropField]
        public const int D = (int)Key.D;
        [PInteropField]
        public const int E = (int)Key.E;
        [PInteropField]
        public const int F = (int)Key.F;
        [PInteropField]
        public const int G = (int)Key.G;
        [PInteropField]
        public const int H = (int)Key.H;
        [PInteropField]
        public const int I = (int)Key.I;
        [PInteropField]
        public const int J = (int)Key.J;
        [PInteropField]
        public const int K = (int)Key.K;
        [PInteropField]
        public const int L = (int)Key.L;
        [PInteropField]
        public const int M = (int)Key.M;
        [PInteropField]
        public const int N = (int)Key.N;
        [PInteropField]
        public const int O = (int)Key.O;
        [PInteropField]
        public const int P = (int)Key.P;
        [PInteropField]
        public const int Q = (int)Key.Q;
        [PInteropField]
        public const int R = (int)Key.R;
        [PInteropField]
        public const int S = (int)Key.S;
        [PInteropField]
        public const int T = (int)Key.T;
        [PInteropField]
        public const int U = (int)Key.U;
        [PInteropField]
        public const int V = (int)Key.V;
        [PInteropField]
        public const int W = (int)Key.W;
        [PInteropField]
        public const int X = (int)Key.X;
        [PInteropField]
        public const int Y = (int)Key.Y;
        [PInteropField]
        public const int Z = (int)Key.Z;
        [PInteropField]
        public const int OPEN_BRACKET = (int)Key.OPEN_BRACKET;
        [PInteropField]
        public const int BACK_SLASH = (int)Key.BACK_SLASH;
        [PInteropField]
        public const int CLOSE_BRACKET = (int)Key.CLOSE_BRACKET;
        [PInteropField]
        public const int NUMPAD0 = (int)Key.NUMPAD0;
        [PInteropField]
        public const int NUMPAD1 = (int)Key.NUMPAD1;
        [PInteropField]
        public const int NUMPAD2 = (int)Key.NUMPAD2;
        [PInteropField]
        public const int NUMPAD3 = (int)Key.NUMPAD3;
        [PInteropField]
        public const int NUMPAD4 = (int)Key.NUMPAD4;
        [PInteropField]
        public const int NUMPAD5 = (int)Key.NUMPAD5;
        [PInteropField]
        public const int NUMPAD6 = (int)Key.NUMPAD6;
        [PInteropField]
        public const int NUMPAD7 = (int)Key.NUMPAD7;
        [PInteropField]
        public const int NUMPAD8 = (int)Key.NUMPAD8;
        [PInteropField]
        public const int NUMPAD9 = (int)Key.NUMPAD9;
        [PInteropField]
        public const int MULTIPLY = (int)Key.MULTIPLY;
        [PInteropField]
        public const int ADD = (int)Key.ADD;
        [PInteropField]
        public const int SEPARATOR = (int)Key.SEPARATOR;
        [PInteropField]
        public const int SUBTRACT = (int)Key.SUBTRACT;
        [PInteropField]
        public const int DECIMAL = (int)Key.DECIMAL;
        [PInteropField]
        public const int DIVIDE = (int)Key.DIVIDE;
        [PInteropField]
        public const int DELETE = (int)Key.DELETE;
        [PInteropField]
        public const int NUM_LOCK = (int)Key.NUM_LOCK;
        [PInteropField]
        public const int SCROLL_LOCK = (int)Key.SCROLL_LOCK;
        [PInteropField]
        public const int F1 = (int)Key.F1;
        [PInteropField]
        public const int F2 = (int)Key.F2;
        [PInteropField]
        public const int F3 = (int)Key.F3;
        [PInteropField]
        public const int F4 = (int)Key.F4;
        [PInteropField]
        public const int F5 = (int)Key.F5;
        [PInteropField]
        public const int F6 = (int)Key.F6;
        [PInteropField]
        public const int F7 = (int)Key.F7;
        [PInteropField]
        public const int F8 = (int)Key.F8;
        [PInteropField]
        public const int F9 = (int)Key.F9;
        [PInteropField]
        public const int F10 = (int)Key.F10;
        [PInteropField]
        public const int F11 = (int)Key.F11;
        [PInteropField]
        public const int F12 = (int)Key.F12;
        [PInteropField]
        public const int F13 = (int)Key.F13;
        [PInteropField]
        public const int F14 = (int)Key.F14;
        [PInteropField]
        public const int F15 = (int)Key.F15;
        [PInteropField]
        public const int F16 = (int)Key.F16;
        [PInteropField]
        public const int F17 = (int)Key.F17;
        [PInteropField]
        public const int F18 = (int)Key.F18;
        [PInteropField]
        public const int F19 = (int)Key.F19;
        [PInteropField]
        public const int F20 = (int)Key.F20;
        [PInteropField]
        public const int F21 = (int)Key.F21;
        [PInteropField]
        public const int F22 = (int)Key.F22;
        [PInteropField]
        public const int F23 = (int)Key.F23;
        [PInteropField]
        public const int F24 = (int)Key.F24;
        [PInteropField]
        public const int PRINTSCREEN = (int)Key.PRINTSCREEN;
        [PInteropField]
        public const int INSERT = (int)Key.INSERT;
        [PInteropField]
        public const int HELP = (int)Key.HELP;
        [PInteropField]
        public const int META = (int)Key.META;
        [PInteropField]
        public const int BACK_QUOTE = (int)Key.BACK_QUOTE;
        [PInteropField]
        public const int QUOTE = (int)Key.QUOTE;
        [PInteropField]
        public const int KP_UP = (int)Key.KP_UP;
        [PInteropField]
        public const int KP_DOWN = (int)Key.KP_DOWN;
        [PInteropField]
        public const int KP_LEFT = (int)Key.KP_LEFT;
        [PInteropField]
        public const int KP_RIGHT = (int)Key.KP_RIGHT;
        [PInteropField]
        public const int DEAD_GRAVE = (int)Key.DEAD_GRAVE;
        [PInteropField]
        public const int DEAD_ACUTE = (int)Key.DEAD_ACUTE;
        [PInteropField]
        public const int DEAD_CIRCUMFLEX = (int)Key.DEAD_CIRCUMFLEX;
        [PInteropField]
        public const int DEAD_TILDE = (int)Key.DEAD_TILDE;
        [PInteropField]
        public const int DEAD_MACRON = (int)Key.DEAD_MACRON;
        [PInteropField]
        public const int DEAD_BREVE = (int)Key.DEAD_BREVE;
        [PInteropField]
        public const int DEAD_ABOVEDOT = (int)Key.DEAD_ABOVEDOT;
        [PInteropField]
        public const int DEAD_DIAERESIS = (int)Key.DEAD_DIAERESIS;
        [PInteropField]
        public const int DEAD_ABOVERING = (int)Key.DEAD_ABOVERING;
        [PInteropField]
        public const int DEAD_DOUBLEACUTE = (int)Key.DEAD_DOUBLEACUTE;
        [PInteropField]
        public const int DEAD_CARON = (int)Key.DEAD_CARON;
        [PInteropField]
        public const int DEAD_CEDILLA = (int)Key.DEAD_CEDILLA;
        [PInteropField]
        public const int DEAD_OGONEK = (int)Key.DEAD_OGONEK;
        [PInteropField]
        public const int DEAD_IOTA = (int)Key.DEAD_IOTA;
        [PInteropField]
        public const int DEAD_VOICED_SOUND = (int)Key.DEAD_VOICED_SOUND;
        [PInteropField]
        public const int DEAD_SEMIVOICED_SOUND = (int)Key.DEAD_SEMIVOICED_SOUND;
        [PInteropField]
        public const int AMPERSAND = (int)Key.AMPERSAND;
        [PInteropField]
        public const int ASTERISK = (int)Key.ASTERISK;
        [PInteropField]
        public const int QUOTEDBL = (int)Key.QUOTEDBL;
        [PInteropField]
        public const int LESS = (int)Key.LESS;
        [PInteropField]
        public const int GREATER = (int)Key.GREATER;
        [PInteropField]
        public const int BRACELEFT = (int)Key.BRACELEFT;
        [PInteropField]
        public const int BRACERIGHT = (int)Key.BRACERIGHT;
        [PInteropField]
        public const int AT = (int)Key.AT;
        [PInteropField]
        public const int COLON = (int)Key.COLON;
        [PInteropField]
        public const int CIRCUMFLEX = (int)Key.CIRCUMFLEX;
        [PInteropField]
        public const int DOLLAR = (int)Key.DOLLAR;
        [PInteropField]
        public const int EURO_SIGN = (int)Key.EURO_SIGN;
        [PInteropField]
        public const int EXCLAMATION_MARK = (int)Key.EXCLAMATION_MARK;
        [PInteropField]
        public const int INVERTED_EXCLAMATION_MARK = (int)Key.INVERTED_EXCLAMATION_MARK;
        [PInteropField]
        public const int LEFT_PARENTHESIS = (int)Key.LEFT_PARENTHESIS;
        [PInteropField]
        public const int NUMBER_SIGN = (int)Key.NUMBER_SIGN;
        [PInteropField]
        public const int PLUS = (int)Key.PLUS;
        [PInteropField]
        public const int RIGHT_PARENTHESIS = (int)Key.RIGHT_PARENTHESIS;
        [PInteropField]
        public const int UNDERSCORE = (int)Key.UNDERSCORE;
        [PInteropField]
        public const int WINDOWS = (int)Key.WINDOWS;
        [PInteropField]
        public const int CONTEXT_MENU = (int)Key.CONTEXT_MENU;
        [PInteropField]
        public const int FINAL = (int)Key.FINAL;
        [PInteropField]
        public const int CONVERT = (int)Key.CONVERT;
        [PInteropField]
        public const int NONCONVERT = (int)Key.NONCONVERT;
        [PInteropField]
        public const int ACCEPT = (int)Key.ACCEPT;
        [PInteropField]
        public const int MODECHANGE = (int)Key.MODECHANGE;
        [PInteropField]
        public const int KANA = (int)Key.KANA;
        [PInteropField]
        public const int KANJI = (int)Key.KANJI;
        [PInteropField]
        public const int ALPHANUMERIC = (int)Key.ALPHANUMERIC;
        [PInteropField]
        public const int KATAKANA = (int)Key.KATAKANA;
        [PInteropField]
        public const int HIRAGANA = (int)Key.HIRAGANA;
        [PInteropField]
        public const int FULL_WIDTH = (int)Key.FULL_WIDTH;
        [PInteropField]
        public const int HALF_WIDTH = (int)Key.HALF_WIDTH;
        [PInteropField]
        public const int ROMAN_CHARACTERS = (int)Key.ROMAN_CHARACTERS;
        [PInteropField]
        public const int ALL_CANDIDATES = (int)Key.ALL_CANDIDATES;
        [PInteropField]
        public const int PREVIOUS_CANDIDATE = (int)Key.PREVIOUS_CANDIDATE;
        [PInteropField]
        public const int CODE_INPUT = (int)Key.CODE_INPUT;
        [PInteropField]
        public const int JAPANESE_KATAKANA = (int)Key.JAPANESE_KATAKANA;
        [PInteropField]
        public const int JAPANESE_HIRAGANA = (int)Key.JAPANESE_HIRAGANA;
        [PInteropField]
        public const int JAPANESE_ROMAN = (int)Key.JAPANESE_ROMAN;
        [PInteropField]
        public const int KANA_LOCK = (int)Key.KANA_LOCK;
        [PInteropField]
        public const int INPUT_METHOD_ON_OFF = (int)Key.INPUT_METHOD_ON_OFF;
        [PInteropField]
        public const int CUT = (int)Key.CUT;
        [PInteropField]
        public const int COPY = (int)Key.COPY;
        [PInteropField]
        public const int PASTE = (int)Key.PASTE;
        [PInteropField]
        public const int UNDO = (int)Key.UNDO;
        [PInteropField]
        public const int AGAIN = (int)Key.AGAIN;
        [PInteropField]
        public const int FIND = (int)Key.FIND;
        [PInteropField]
        public const int PROPS = (int)Key.PROPS;
        [PInteropField]
        public const int STOP = (int)Key.STOP;
        [PInteropField]
        public const int COMPOSE = (int)Key.COMPOSE;
        [PInteropField]
        public const int ALT_GRAPH = (int)Key.ALT_GRAPH;
        [PInteropField]
        public const int BEGIN = (int)Key.BEGIN;
        [PInteropField]
        public const int UNDEFINED = (int)Key.UNDEFINED;
    }
}