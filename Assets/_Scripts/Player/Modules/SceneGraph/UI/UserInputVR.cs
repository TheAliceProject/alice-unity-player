using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputVR : UserInput
{
    public enum VRButtonType
    {
        Normal,
        Shift,
        Space,
        Backspace,
        Caps
    }
    public List<VRUIButton> keyboardButtons = null;
    private bool capslockOn = false;

    public void AddToInputField(string s)
    {
        string currText = inputField.text;
        inputField.text = currText + s;

        if(!capslockOn)
            SetShifted(false, false);
    }

    public void Backspace()
    {
        if(inputField.text.Length > 0)
        {
            string currText = inputField.text;
            inputField.text = currText.Substring(0, currText.Length - 1);
        }
    }

    public void SetShifted(bool on, bool capsLock)
    {
        capslockOn = capsLock;
        for (int i = 0; i < keyboardButtons.Count; i++)
        {
            keyboardButtons[i].SetShifted(on);
        }
    }
}
